using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
namespace T3
{
    public partial class Frm_Padre : Form
    {
        Clases._Cls_RutinasImpresion _rutinasImpresion = new Clases._Cls_RutinasImpresion();
        CLASES._Cls_Varios_Metodos myUtilidad = new CLASES._Cls_Varios_Metodos(true);
        public Form _Frm_Security = new Form();
        public Frm_Contenedor _Frm_Contenedor;
        public static string _Str_Comp = "";
        public static string _Str_GroupComp = "";
        public static string _Str_Use = "";
        public static string _Str_UserGroup = "";
        public static bool _Bol_ClaveMaestra=false;
        public static int _Int_UserRestricPing = 0;
        public WaitCallback _async_Default;
        public bool _Bol_Cerrar = true;
        int _Int_i = 0;
        public Frm_Padre()
        {
            InitializeComponent();
            this.Text = "Sistema T3 - " + CLASES._Cls_Conexion._G_Str_VersionT3;
        }
        string _Str_TituloPadre = "";
        
        private string _Mtd_ObtenerIP()
        {
            //return Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString().Trim();
            string _Str_Host = System.Net.Dns.GetHostName();
            string _Str_IP = "";

            for (int i = 0; i <= System.Net.Dns.GetHostEntry(_Str_Host).AddressList.Length - 1; i++)
            {
                _Str_IP = System.Net.Dns.GetHostEntry(_Str_Host).AddressList[i].ToString();

                // primero evalua si existe un IP estandar de la red SODICA
                if (_Str_IP.IndexOf("192.168.0.") != -1) return _Str_IP; // denca
                if (_Str_IP.IndexOf("192.168.1.") != -1) return _Str_IP; // conssa
                if (_Str_IP.IndexOf("192.168.2.") != -1) return _Str_IP; // mcy
                if (_Str_IP.IndexOf("192.168.3.") != -1) return _Str_IP; // mcbo
                if (_Str_IP.IndexOf("192.168.4.") != -1) return _Str_IP; // scb
                if (_Str_IP.IndexOf("192.168.5.") != -1) return _Str_IP; // pzo
                if (_Str_IP.IndexOf("192.168.6.") != -1) return _Str_IP; // bna
                if (_Str_IP.IndexOf("192.168.7.") != -1) return _Str_IP; // val
                if (_Str_IP.IndexOf("192.168.8.") != -1) return _Str_IP; // bqto
                if (_Str_IP.IndexOf("192.168.9.") != -1) return _Str_IP; // ccs
                if (_Str_IP.IndexOf("192.168.10.") != -1) return _Str_IP; // bnas

                if (_Str_IP.IndexOf("192.168.11.") != -1) return _Str_IP; // ¿futuro?
                if (_Str_IP.IndexOf("192.168.12.") != -1) return _Str_IP; // ¿futuro?
                if (_Str_IP.IndexOf("192.168.13.") != -1) return _Str_IP; // ¿futuro?
                if (_Str_IP.IndexOf("192.168.14.") != -1) return _Str_IP; // ¿futuro?
                if (_Str_IP.IndexOf("192.168.15.") != -1) return _Str_IP; // ¿futuro?
            }

            // si no encuentra un IP estándar, entonces devuelve el primero que no sea IPV6
            for (int i = 0; i <= System.Net.Dns.GetHostEntry(_Str_Host).AddressList.Length - 1; i++)
            {
                if (System.Net.Dns.GetHostEntry(_Str_Host).AddressList[i].IsIPv6LinkLocal == false)
                {
                    _Str_IP = System.Net.Dns.GetHostEntry(_Str_Host).AddressList[i].ToString();
                }
            }

            return _Str_IP;
        }

        public Frm_Padre(string _P_Str_Company, string _P_Str_User, Form _P_Frm_Security)
        {
            InitializeComponent();
            this.Text = "Sistema T3 - " + CLASES._Cls_Conexion._G_Str_VersionT3;
            string _Str_Cadena = "Select RTRIM(cabreviado) COLLATE DATABASE_DEFAULT+' - '+LTRIM(cname) AS cname from TCOMPANY where ccompany='" + _P_Str_Company + "'";
            DataSet _Ds = Program._MyClsCnn._mtd_conexion._Mtd_RetornarDataset(_Str_Cadena);
            if (_Ds.Tables[0].Rows.Count > 0)
            {
                this.statusBarPanel3.Text = _P_Str_Company.Trim().ToUpper() + " - " + _Ds.Tables[0].Rows[0][0].ToString().Trim().ToUpper();
            }
            _Str_Cadena = "Select cname,cgroup from TUSER where cuser='" + _P_Str_User + "'";
            _Ds = Program._MyClsCnn._mtd_conexion._Mtd_RetornarDataset(_Str_Cadena);
            if (_Ds.Tables[0].Rows.Count > 0)
            {
                this.statusBarPanel2.Text = _P_Str_User.Trim().ToUpper() + " - " + _Ds.Tables[0].Rows[0][0].ToString().Trim().ToUpper();
                _Str_UserGroup = _Ds.Tables[0].Rows[0][1].ToString().Trim().ToUpper();
            }
            _Str_TituloPadre = this.Text;
            _Str_Cadena = "SELECT cgroupcomp FROM TGROUPCOMPANYD where ccompany='" + _P_Str_Company + "'";
            _Ds = Program._MyClsCnn._mtd_conexion._Mtd_RetornarDataset(_Str_Cadena);
            if (_Ds.Tables[0].Rows.Count > 0)
            {
                _Str_GroupComp = _Ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            try
            {
                statusBarPanel6.Text = "Dirección IP: " + myUtilidad._Mtd_ObtenerIP();
            }
            catch { statusBarPanel6.Text = "Dirección IP: No se obtuvo"; }

            _Frm_Security = _P_Frm_Security;
            _Str_Comp = _P_Str_Company;
            _Str_Use = _P_Str_User;
        }
        private delegate void UpdateStatusDelegate();
        public void _Mtd_BloquearMenu()
        {
            for (int _I = 1; _I < menuStrip1.Items.Count; _I++)
            {
                menuStrip1.Items[_I].Enabled = false;
            }
            sistemaToolStripMenuItem.DropDownItems[0].Enabled = false;
        }        
        private void _Mtd_Revisar_Hijos(ToolStripMenuItem menuhijo)
        {
            if (menuhijo.DropDownItems.Count > 0)
            {
                int contador = menuhijo.DropDownItems.Count;
                int contador_items = 0;
                try
                {
                    while (contador != 0)
                    {
                        if (menuhijo.DropDownItems[contador_items].GetType() != typeof(System.Windows.Forms.ToolStripSeparator))
                        {
                            _Mtd_ValidarMenu((System.Windows.Forms.ToolStripMenuItem)menuhijo.DropDownItems[contador_items]);
                            _Mtd_Revisar_Hijos((System.Windows.Forms.ToolStripMenuItem)menuhijo.DropDownItems[contador_items]);
                        }
                        contador--;
                        contador_items++;
                    }
                }
                catch
                {

                }
            }
        }
        DataSet _Ds_Menu;
        delegate void SetToolStripMenuItemCallback(ToolStripMenuItem menuItem);
        private void _Mtd_ValidarMenu(ToolStripMenuItem menuItem)
        {
            //string _Str_SentenciaSQL = "select * from Vst_MenuGroup";
            if (_Ds_Menu == null)
            {
                _Ds_Menu = Program._MyClsCnn._mtd_conexion._Mtd_RetornarDataset("SELECT * FROM TMENUGROUP WHERE cgroup='" + _Str_UserGroup + "'");
            }
            if (_Ds_Menu.Tables.Count > 0)
            {
                //_Ds_Menu.ReadXml(Application.StartupPath + "/Menu.xml");
                DataRow[] _Dtw_Rows = _Ds_Menu.Tables[0].Select("c_menuId='" + menuItem.Name + "'");
                foreach (DataRow filas in _Dtw_Rows)
                {
                    if (filas["c_menuId"].ToString().TrimEnd() == menuItem.Name)
                    {
                        if (filas["c_habilitado"].ToString().TrimEnd() == "0")
                        {
                            if (InvokeRequired)
                            {
                                SetToolStripMenuItemCallback _Sets = new SetToolStripMenuItemCallback(_Mtd_ValidarMenu);
                                this.Invoke(_Sets, new object[] { menuItem });
                            }
                            else
                            {
                                menuItem.Enabled = false;
                            }
                        }
                    }
                }
            }
        }
        public static bool _Bol_TabsP = false;
        private void _Mtd_MenuItems()
        {
            if (Frm_Inicio._Bol_CrearMenu)
            {
                if (!CLASES._Cls_Conexion._Bol_ConexionRemota)
                {
                    _Mtd_Crear_menu();
                }
                Frm_Inicio._Bol_CrearMenu = false;
            }
            foreach (ToolStripMenuItem itm_1 in menuStrip1.Items)
            {
                _Mtd_ValidarMenu(itm_1);
                _Mtd_Revisar_Hijos(itm_1);                
            }
            _Bol_TabsP = true;
        }
        //-------------------------------
        private void _Mtd_Validar_Guardar(string _P_Str_menuId, string _P_Str_Padre_Id, string _P_Str_Nombre)
        {
            DataSet _Ds = Program._MyClsCnn._mtd_conexion._Mtd_RetornarDataset("select * from TMENU where c_menuId='" + _P_Str_menuId + "'");
            if (_Ds.Tables[0].Rows.Count == 0)
            {
                string _Str_SentenciaSQL = "insert into TMENU(c_menuId,c_padreId,c_nameMenu) values ('" + _P_Str_menuId + "','" + _P_Str_Padre_Id + "','" + _P_Str_Nombre + "')";
                Program._MyClsCnn._mtd_conexion._Mtd_EjecutarSentencia(_Str_SentenciaSQL);
            }
        }
        private void _Mtd_Guarda_hijos(ToolStripMenuItem menuhijo)
        {
            if (menuhijo.DropDownItems.Count > 0)
            {
                int contador = menuhijo.DropDownItems.Count;
                int contador_items = 0;
                try
                {
                    while (contador != 0)
                    {
                        if (menuhijo.DropDownItems[contador_items].GetType() != typeof(System.Windows.Forms.ToolStripSeparator))
                        {
                            _Mtd_Validar_Guardar(((System.Windows.Forms.ToolStripMenuItem)menuhijo.DropDownItems[contador_items]).Name, menuhijo.Name, ((System.Windows.Forms.ToolStripMenuItem)menuhijo.DropDownItems[contador_items]).Text);
                            _Mtd_Guarda_hijos((System.Windows.Forms.ToolStripMenuItem)menuhijo.DropDownItems[contador_items]);
                        }
                        contador--;
                        contador_items++;
                    }
                }
                catch (Exception oo)
                {
                    MessageBox.Show(oo.ToString());
                }
            }
        }
        private void _Mtd_Crear_menu()
        {
            try
            {
                foreach (ToolStripMenuItem itm_1 in this.menuStrip1.Items)
                {
                    _Mtd_Validar_Guardar(itm_1.Name, itm_1.Name, itm_1.Text);
                    _Mtd_Guarda_hijos(itm_1);
                }
            }
            catch (Exception ou)
            {
                string eror = ou.Message.ToString();
            }
        }
        //-------------------------------
        public bool _Mtd_AbiertoOno(Form _Frm_Formulario)
        {
            foreach (Form _Frm_Hijo in this.MdiChildren)
            {
                if (_Frm_Hijo.Name == _Frm_Formulario.Name)
                {
                    _Frm_Hijo.Activate();
                    return true;
                }
            }
            return false;
        }
        private bool _Mtd_VerificarCierreCaja()
        {
            foreach (Form _Frm in Application.OpenForms)
            {
                if (_Frm.Name.Trim() == "Frm_ConsultaCajaAbierta")
                {
                    return true;
                }
            }
            return false;
        }
        bool _G_Bol_Frm_ControlImpresion_Close = true;
        public void _Mtd_ShowForm_Advertencia()
        {
            if (_G_Bol_Frm_ControlImpresion_Close)
            {
                if (!_Mtd_VerificarCierreCaja())
                {
                    Frm_ControlImpresion _Frm = new Frm_ControlImpresion(this);
                    if (_Frm._Mtd_MostrarFormulario())
                    {
                        _Frm.FormClosing += new FormClosingEventHandler(Frm_ControlImpresion_FormClosing);
                        System.Drawing.Rectangle _rect = SystemInformation.VirtualScreen;
                        _Frm.Height = _rect.Height;
                        _Frm.Width = _rect.Width;
                        _Frm.Show();
                        _Frm.Focus();
                        _Frm.BringToFront();
                    }
                    else
                    {
                        _Frm.Close();
                    }
                    _G_Bol_Frm_ControlImpresion_Close = false;
                }
            }
        }
        private void Frm_ControlImpresion_FormClosing(object sender, FormClosingEventArgs e)
        {
            _G_Bol_Frm_ControlImpresion_Close = true;
        }

        public void _Mtd_InicializarT3()
        {
            CLASES._Cls_Varios_Metodos._Mtd_Help(this);
            Thread _Thr_Thread = new Thread(new ThreadStart(_Mtd_MenuItems));
            _Thr_Thread.Start();
            while (!_Thr_Thread.IsAlive) ;
            Frm_WaitForm _Frm_Form = new Frm_WaitForm(1000, _Thr_Thread, "Configurando menú...");
            _Frm_Form.ShowDialog(this);
            _Frm_Form.Dispose();
            _Frm_Contenedor = new Frm_Contenedor(this);
            _Frm_Contenedor.MdiParent = this;
            _Frm_Contenedor.Dock = DockStyle.Fill;
            _Frm_Contenedor.Show();
            _Frm_Contenedor._Tm_Tiempo.Enabled = true;
            if (!_Bol_ClaveMaestra)
            {
                if (_Int_UserRestricPing > 0)
                {
                    _Tm_TiempoUserRestric.Enabled = true;
                    _Tm_TiempoUserRestric.Interval = _Int_UserRestricPing * 1000;
                }
            }
            this.Text = _Str_TituloPadre + " - " + Frm_Padre._Str_Use + " - " + CLASES._Cls_Varios_Metodos._Mtd_PrefComp(Frm_Padre._Str_Comp) + " - " + CLASES._Cls_Varios_Metodos._Mtd_NombComp(Frm_Padre._Str_Comp);
            if (Frm_Padre._Str_Comp.Trim() == "S01" | Frm_Padre._Str_Comp.Trim() == "S05" | Frm_Padre._Str_Comp.Trim() == "S06" | Frm_Padre._Str_Comp.Trim() == "S07" | Frm_Padre._Str_Comp.Trim() == "S072" | Frm_Padre._Str_Comp.Trim() == "S03" | Frm_Padre._Str_Comp.Trim() == "S02" | Frm_Padre._Str_Comp.Trim() == "S042" | Frm_Padre._Str_Comp.Trim() == "S04" | Frm_Padre._Str_Comp.Trim() == "S08")
            { this._Ctrl_Buscar1._PBox_Logo.Image = T3.Properties.Resources.LOGO_SODICA; }
            else
            { this._Ctrl_Buscar1._PBox_Logo.Image = T3.Properties.Resources.LOGO_MOGOSA; }
            this._Ctrl_Buscar1._PBox_Logo.Visible = true;

            //fecha de cierre de venta
            this._Ctrl_Buscar1._Lbl_FechaCierreVenta.Text = "Cierre de Ventas : " + _Mtd_UltimaFechaVentas();
            this._Ctrl_Buscar1._Lbl_FechaCierreVenta.Visible = true;

            //Validacion de Impresoras
            if (myUtilidad._Mtd_UsuarioProceso(Frm_Padre._Str_Use, "F_VERIFICAR_IMPRESORAS"))
            {
                if (_rutinasImpresion._Mtd_EstaHabilitadoConfiguracionImpresion())
                {
                    //Verifico que el computador tenga instaldo todas las impresoras
                    _rutinasImpresion._Mtd_ExistenTodasLasImpresorasConfiguradas();
                }
            }
        }


        public static string _Mtd_UltimaFechaVentas()
        {
            string dtUltimaFechaVentas = "";
            try
            {
                string _Str_SQL = "SELECT dbo.FNC_ULTIMAFECHAVENTAS('" + DateTime.Now.ToShortDateString() + "','" + Frm_Padre._Str_Comp + "') as UltimaFechaVentas";
                DataSet _Ds_DataSet = new DataSet();
                _Ds_DataSet = Program._MyClsCnn._mtd_conexion._Mtd_RetornarDataset(_Str_SQL);
                if (_Ds_DataSet.Tables[0].Rows.Count > 0)
                {
                    dtUltimaFechaVentas = Convert.ToDateTime(_Ds_DataSet.Tables[0].Rows[0][0]).ToShortDateString();
                }
            }
            catch (Exception _Excepcion)
            {
            }
            return dtUltimaFechaVentas;
        }


        private void Frm_Padre_Load(object sender, EventArgs e)
        {
            _Mtd_InicializarT3();
            this.Bounds = Screen.PrimaryScreen.Bounds;
            _Pnl_Espera.Left = this.Width - (_Pnl_Espera.Width + 20);
            _async_Default = new WaitCallback(_Mtd_UserRestricPing);
            _Mtd_AbrirPresentacion();
        }
        private void _Mtd_UserRestricPing(Object param)
        {
            try
            {
                if (!Frm_Padre._Bol_ClaveMaestra)
                {
                    string _Str_SQL = "SELECT CUSER FROM TUSERONLINE WHERE CUSER='" + Frm_Padre._Str_Use + "' AND CIP='" + _Mtd_ObtenerIP() + "'";
                    DataSet _Ds = Program._MyClsCnn._mtd_conexion._Mtd_RetornarDataset(_Str_SQL);
                    if (_Ds.Tables[0].Rows.Count > 0)
                    {
                        _Str_SQL = "UPDATE TUSERONLINE SET cfechahoraping=GETDATE() WHERE CUSER='" + Frm_Padre._Str_Use + "' AND CIP='" + _Mtd_ObtenerIP() + "'";
                        Program._MyClsCnn._mtd_conexion._Mtd_EjecutarSentencia(_Str_SQL);
                    }
                    else
                    {
                        _Str_SQL = "INSERT INTO TUSERONLINE(CUSER,CIP,CFECHAHORAPING,CVERSIONT3) VALUES ('" + Frm_Padre._Str_Use + "','" + _Mtd_ObtenerIP() + "',GETDATE(),'" + T3.CLASES._Cls_Conexion._G_Str_VersionT3 + "')";
                        Program._MyClsCnn._mtd_conexion._Mtd_EjecutarSentencia(_Str_SQL);
                    }
                }
            }
            catch
            {

            }
        }
        private void _Mtd_AbrirPresentacion()
        {
            string _Str_Cadena = "SELECT c_informativoconssa FROM TCOMPANY WHERE ccompany='" + Frm_Padre._Str_Comp + "' AND c_informativoconssa='1'";
            if (Program._MyClsCnn._mtd_conexion._Mtd_RetornarDataset(_Str_Cadena).Tables[0].Rows.Count > 0)
            {
                Frm_Presentacion _Frm = new Frm_Presentacion();
                System.Drawing.Rectangle _rect = SystemInformation.VirtualScreen;
                _Frm.Height = _rect.Height;
                _Frm.Width = _rect.Width;
                _Frm.Show();
                _Frm.Focus();
                _Frm.BringToFront();
                //System.Diagnostics.Process.Start("http://goo.gl/TP11");
            }
        }
        private void Frm_Padre_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_Bol_Cerrar)
            {
                if (MessageBox.Show("¿Está seguro de que desea salir del sistema?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    //myUtilidad._Mtd_Cerrar_T3_Popup("T3BUGS");
                    myUtilidad._Mtd_Cerrar_T3_Popup("openvpn");
                    myUtilidad._Mtd_Cerrar_T3_Popup("openvpn-gui-1.0.3");
                    Cursor = Cursors.Default;
                    _Mtd_EliminarUsuarioRestric(Frm_Padre._Str_Use);
                    _Bol_Cerrar = false; Application.Exit();
                }
                else
                { e.Cancel = true; }
            }
            else
            {
                _Mtd_EliminarUsuarioRestric(Frm_Padre._Str_Use);
            }
        }
        private void _Mtd_EliminarUsuarioRestric(string _Str_User)
        {
            try
            {
                if (!Frm_Padre._Bol_ClaveMaestra)
                {
                    string _Str_SQL = "DELETE FROM TUSERONLINE WHERE CUSER='" + _Str_User + "' AND CIP='"+_Mtd_ObtenerIP()+"'";
                    Program._MyClsCnn._mtd_conexion._Mtd_EjecutarSentencia(_Str_SQL);
                }
            }
            catch
            {
            }
        }
        private void usuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_UsuarioAdmin _Frm_G = new Frm_UsuarioAdmin();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }
        private void grupoDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_GrupoUsurario _Frm_G = new Frm_GrupoUsurario();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }
        private void menúDelUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void tabsPorUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Tabs _Frm = new Frm_Tabs();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _Bol_Cerrar = false;
            this.Close();
            Cursor = Cursors.WaitCursor;
            //myUtilidad._Mtd_Cerrar_T3_Popup("T3BUGS");
            Cursor = Cursors.Default;
            _Frm_Security.Show();
        }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void consultaDeProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Proveedores _Frm_G = new Frm_Proveedores();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Busqueda2 _Frm = new Frm_Busqueda2(3);
                _Frm.Name = "Frm1";
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                //aebb

                Frm_Navegador _Frm = new Frm_Navegador(CLASES._Cls_Varios_Metodos._Str_Servidor_Web + "/default.aspx", false);                

                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void menuItem7_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                //aebb    
                string _Str_Web1 = CLASES._Cls_Varios_Metodos._Str_Servidor_Web + "/pocvista.aspx?" + osio.encriptar("CodPoc") + "=";                        
                string _Str_Web2 = "&" + osio.encriptar("CodComp") + "=" + osio.encriptar(Frm_Padre._Str_Comp);
                ToolStripMenuItem[] _Tsm_Menu = new ToolStripMenuItem[3];
                _Tsm_Menu[0] = new ToolStripMenuItem("Id P.O.C.");
                _Tsm_Menu[1] = new ToolStripMenuItem("Fecha");
                _Tsm_Menu[2] = new ToolStripMenuItem("Proveedor");
                string[] _Str_Campos = new string[3];
                _Str_Campos[0] = "cpreoc";
                _Str_Campos[1] = "cfecha";
                _Str_Campos[2] = "c_nomb_abreviado";
                string _Str_Cadena = "SELECT T3TPREORDENCM.cpreoc as 'Id P.O.C.',Convert(varchar,T3TPREORDENCM.cfecha,103) AS Fecha, T3TPROVEEDOR.c_nomb_abreviado as Proveedor " +
    "FROM T3TPREORDENCM INNER JOIN " +
    "T3TPROVEEDOR ON T3TPREORDENCM.cproveedor = T3TPROVEEDOR.cproveedor AND T3TPREORDENCM.ccompany = T3TPROVEEDOR.ccompany AND " +
    "T3TPREORDENCM.cdelete = T3TPROVEEDOR.cdelete " +
    "WHERE (T3TPREORDENCM.cdelete = 0) AND (T3TPREORDENCM.cstatus = 3) AND (T3TPREORDENCM.ccompany = '" + Frm_Padre._Str_Comp + "')";
                Frm_Busqueda _Frm = new Frm_Busqueda(_Str_Cadena, _Str_Campos, "Preorden de Compra", _Tsm_Menu, _Str_Web1, _Str_Web2, false);
                _Frm.Name = "Frm3";
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                //aebb
                string _Str_Web1 = CLASES._Cls_Varios_Metodos._Str_Servidor_Web + "/ocvista.aspx?" + osio.encriptar("CodOc") + "=";                
            
                               
                string _Str_Web2 = "&" + osio.encriptar("CodComp") + "=" + osio.encriptar(Frm_Padre._Str_Comp);
                ToolStripMenuItem[] _Tsm_Menu = new ToolStripMenuItem[3];
                _Tsm_Menu[0] = new ToolStripMenuItem("Id O.C.");
                _Tsm_Menu[1] = new ToolStripMenuItem("Fecha");
                _Tsm_Menu[2] = new ToolStripMenuItem("Proveedor");
                string[] _Str_Campos = new string[3];
                _Str_Campos[0] = "cnumoc";
                _Str_Campos[1] = "cfechaoc";
                _Str_Campos[2] = "c_nomb_abreviado";
                string _Str_Cadena = "SELECT cnumoc AS [Id O.C.],Fecha,Proveedor,Cajas,Monto " +
    "FROM vst_tabordencompra " +
    "WHERE (ccompany = '" + Frm_Padre._Str_Comp + "')";
                Frm_Busqueda _Frm = new Frm_Busqueda(_Str_Cadena, _Str_Campos, "Orden de Compra", _Tsm_Menu, _Str_Web1, _Str_Web2, true);
                _Frm.Name = "Frm4";
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void notaDeRecepciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_NotaRecepcion _Frm = new Frm_NotaRecepcion();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void facturasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_FacturasCargadas _Frm = new Frm_FacturasCargadas(false);
            if (!_Mtd_AbiertoOno(_Frm))
            { _Frm.MdiParent = this; _Frm.Show(); }
            else
            { _Frm.Dispose(); }
        }
        private void notaDeEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_NotaEntrega _Frm = new Frm_NotaEntrega();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripSeparator7_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsolidaObligaciones _Frm_ConsolidaObligaciones = new Frm_ConsolidaObligaciones();
                if (!_Mtd_AbiertoOno(_Frm_ConsolidaObligaciones))
                { _Frm_ConsolidaObligaciones.MdiParent = this; _Frm_ConsolidaObligaciones.Dock = DockStyle.Fill; _Frm_ConsolidaObligaciones.Show(); }
                else
                { _Frm_ConsolidaObligaciones.Dispose(); }
            }
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_RelPagProv _Frm = new Frm_RelPagProv();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void ordenesDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_OrdenPago _Frm = new Frm_OrdenPago();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void chequesEmitidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_EmisionCheque _Frm = new Frm_EmisionCheque();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void notsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_NotaCredito _Frm = new Frm_NotaCredito();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                //{ _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void notasDeDébitoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_NotaDebito _Frm = new Frm_NotaDebito();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_AnulNotaCredito _Frm = new Frm_AnulNotaCredito();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_AnulNotaDebito _Frm = new Frm_AnulNotaDebito();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_CompRetencion _Frm = new Frm_CompRetencion();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ComprobISLR _Frm = new Frm_ComprobISLR();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void reporteDeParametrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Cursor = Cursors.WaitCursor;
                REPORTESS _Frm = new REPORTESS(new string[] { "VST_ISLR" }, "", "T3.Report.rTablaISRL", "", "", "", "", "0=0");
                _Frm.MdiParent = this;
                Cursor = Cursors.Default;
                _Frm.Show();
            }
        }
        private void pruebaFormulaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Formulas _Frm = new Frm_Formulas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void parametrosLimitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ParLimPago _Frm = new Frm_ParLimPago();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void recepciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Recepcion _Frm = new Frm_Recepcion(true);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void transporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Ttransporte _Frm = new Frm_Ttransporte();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void transportistaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Transportista _Frm = new Frm_Transportista();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void tipoDeTransporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_TipoTransporte _Frm = new Frm_TipoTransporte();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem25_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_RutaDespacho _Frm = new Frm_RutaDespacho();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_EstatusBackOrder _Frm = new Frm_EstatusBackOrder();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem28_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Cursor = Cursors.WaitCursor;
                Frm_ControlDespacho _Frm = new Frm_ControlDespacho(1);
                Cursor = Cursors.Default;
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem30_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ControlFactura _Frm = new Frm_ControlFactura();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem33_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ControlTransporte _Frm = new Frm_ControlTransporte();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_TransferMME _Frm = new Frm_TransferMME();
                _Frm.Name = "Frm_TransferMME1";
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void devolucionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No puede trabajar en esta opción, por favor informar a CONSSA!!", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            //{
            //    Frm_DevolCompra _Frm = new Frm_DevolCompra();
            //    if (!_Mtd_AbiertoOno(_Frm))
            //    { _Frm.MdiParent = this; _Frm.Show(); }
            //    else
            //    { _Frm.Dispose(); }
            //}
        }
        private void consultaDePedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaMultiple _Frm = new Frm_ConsultaMultiple(_Enu_TiposConsultas.Pedido);
                //Frm_ConsultaPedidos _Frm = new Frm_ConsultaPedidos();

                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem29_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                //Frm_ConsultaPreFactura _Frm = new Frm_ConsultaPreFactura();
                Frm_ConsultaMultiple _Frm = new Frm_ConsultaMultiple(_Enu_TiposConsultas.Prefactura);
                
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void vendedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Vendedores _Frm = new Frm_Vendedores();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void zonasDeVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Zonadeventas _Frm = new Frm_Zonadeventas();
                if (!_Mtd_AbiertoOno(_Frm))                
                    { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void clientesPorZonaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ZonaporCliente _Frm = new Frm_ZonaporCliente();
                if (!_Mtd_AbiertoOno(_Frm))
                //{ _Frm.MdiParent = this; _Frm.Show(); }
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }                    
                else
                { _Frm.Dispose(); }
            }
        }
        private void frmVentaporProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ZonaporVendedor _Frm = new Frm_ZonaporVendedor();
                if (!_Mtd_AbiertoOno(_Frm))
                //{ _Frm.MdiParent = this; _Frm.Show(); }
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }   
                else
                { _Frm.Dispose(); }
            }
        }
        private void cuotaDeVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Cuotaventas1 _Frm_CuotaVentas = new Frm_Cuotaventas1();
                if (!_Mtd_AbiertoOno(_Frm_CuotaVentas))
                { _Frm_CuotaVentas.MdiParent = this; _Frm_CuotaVentas.Dock = DockStyle.Fill; _Frm_CuotaVentas.Show(); }
                else
                { _Frm_CuotaVentas.Dispose(); }
            }
        }
        private void rutaDeVisitasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_RutaVisitas _Frm = new Frm_RutaVisitas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void rutaDeVisitasTransferenciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_RutaVisitasTransferencia _Frm = new Frm_RutaVisitasTransferencia();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void cargaDePedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                string _Str_Cadena = "Select cpassw from TUSER where cuser='" + Frm_Padre._Str_Use.Trim() + "' and cdelete='0'";
                DataSet _Ds = Program._MyClsCnn._mtd_conexion._Mtd_RetornarDataset(_Str_Cadena);
                if (_Ds.Tables[0].Rows.Count > 0)
                {
                    string _Str_Web1 = CLASES._Cls_Varios_Metodos._Str_Servidor_Web_2 + "/Frm_CargaPedidos.aspx?" + osio.encriptar("ccompany") + "=" + osio.encriptar(Frm_Padre._Str_Comp); 
                    Frm_Navegador _Frm = new Frm_Navegador(_Str_Web1, false);
                    if (!_Mtd_AbiertoOno(_Frm))
                    { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                    else
                    { _Frm.Dispose(); }
                }
            }
        }
        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_GrupodeVentas _Frm = new Frm_GrupodeVentas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void grupoDeVentasPorProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_VentaporProveedor _Frm = new Frm_VentaporProveedor();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void consultaDeClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                //Frm_Clientes_VC _Frm = new Frm_Clientes_VC();
                Frm_Clientes_VC_1 _Frm = new Frm_Clientes_VC_1();
                //_Frm.Name = "Frm7";
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }

        }
        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_EstatusClientes _Frm = new Frm_EstatusClientes();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void límitesDeCréditoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Busqueda2 _Frm = new Frm_Busqueda2(11);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void toolStripMenuItem27_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_LimiteCreditoAutorizaN _Frm = new Frm_LimiteCreditoAutorizaN();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void consultaDeClientesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Clientes_VC_1 _Frm = new Frm_Clientes_VC_1();
                //_Frm.Name = "Frm7";
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void cargaDeRelaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                //string _Str_Url = CLASES._Cls_Varios_Metodos._Str_Servidor_Web + "mrelacionWin.aspx?" + osio.encriptar("usuario") + "=" + osio.encriptar(Frm_Padre._Str_Use) + "&" + osio.encriptar("compania") + "=" + osio.encriptar(Frm_Padre._Str_Comp);
                string _Str_Url = CLASES._Cls_Varios_Metodos._Str_Servidor_Web + "Frm_RelacionWin.aspx?" + osio.encriptar("usuario") + "=" + osio.encriptar(Frm_Padre._Str_Use) + "&" + osio.encriptar("compania") + "=" + osio.encriptar(Frm_Padre._Str_Comp);
                Frm_Navegador _Frm = new Frm_Navegador(_Str_Url, true);
                _Frm.MdiParent = this;
                _Frm.Dock = DockStyle.Fill;
                _Frm.Show();
            }
        }
        private void nCClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_NotaCreditoCxC _Frm = new Frm_NotaCreditoCxC();
                if (!_Mtd_AbiertoOno(_Frm))                
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void nDClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_NotaDebitoCxC _Frm = new Frm_NotaDebitoCxC();
                if (!_Mtd_AbiertoOno(_Frm))                
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void anulaciónDeNCClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_NotaCreditoCxCAnul _Frm = new Frm_NotaCreditoCxCAnul();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void anulaciónDeNDClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_NotaDebitoCxCAnul _Frm = new Frm_NotaDebitoCxCAnul();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void procesosContablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ProcesosContables Frm_ProcesosContables1 = new Frm_ProcesosContables();
                if (!_Mtd_AbiertoOno(Frm_ProcesosContables1))
                { Frm_ProcesosContables1.MdiParent = this;Frm_ProcesosContables1.Dock = DockStyle.Fill; Frm_ProcesosContables1.Show(); }                
                else
                { Frm_ProcesosContables1.Dispose(); }
            }
        }
        private void tiposDeComprobantesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Comprobante Frm_COMPROBANTE1 = new Frm_Comprobante();
                if (!_Mtd_AbiertoOno(Frm_COMPROBANTE1))
                { Frm_COMPROBANTE1.MdiParent = this; Frm_COMPROBANTE1.Show(); }
                else
                { Frm_COMPROBANTE1.Dispose(); }
            }
        }
        private void consultaDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaProductos _Frm = new Frm_ConsultaProductos();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void totalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                if (myUtilidad._Mtd_UsuarioProceso(Frm_Padre._Str_Use, "F_IMPRESION_TARJETA"))
                {
                    string _Str_Cadena = "Select ciniciado from TINVFISICOM where ccompany='" + Frm_Padre._Str_Use + "'";
                    DataSet _Ds = Program._MyClsCnn._mtd_conexion._Mtd_RetornarDataset(_Str_Cadena);
                    if (_Ds.Tables[0].Rows.Count > 0)
                    {
                        if (_Ds.Tables[0].Rows[0][0].ToString().Trim() == "1")
                        { MessageBox.Show("Se ha iniciado el conteo de inventario físico. No podra realizar operaciones en este módulo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                        else
                        {
                            Frm_ConteoCompleto _Frm = new Frm_ConteoCompleto();
                            if (!_Mtd_AbiertoOno(_Frm))
                            { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                            else
                            { _Frm.Dispose(); }
                        }
                    }
                    else
                    {
                        Frm_ConteoCompleto _Frm = new Frm_ConteoCompleto();
                        if (!_Mtd_AbiertoOno(_Frm))
                        { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                        else
                        { _Frm.Dispose(); }
                    }
                }
                else
                { MessageBox.Show("Su usuario no posee permiso para entrar en este módulo", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
            }
        }
        private void cargarConteoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                if (myUtilidad._Mtd_UsuarioProceso(Frm_Padre._Str_Use, "F_IMPRESION_TARJETA"))
                {
                    string _Str_Cadena = "Select cimpreso,ciniciado from TINVFISICOM where ccompany='" + Frm_Padre._Str_Comp + "'";
                    DataSet _Ds = Program._MyClsCnn._mtd_conexion._Mtd_RetornarDataset(_Str_Cadena);
                    if (_Ds.Tables[0].Rows.Count > 0)
                    {
                        if (_Ds.Tables[0].Rows[0][0].ToString().Trim() == "1")
                        {
                            if (_Ds.Tables[0].Rows[0][1].ToString().Trim() == "1")
                            {
                                Frm_ConteoInventario _Frm = new Frm_ConteoInventario();
                                if (!_Mtd_AbiertoOno(_Frm))
                                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                                else
                                { _Frm.Dispose(); }
                            }
                            else
                            {
                                Frm_VerificacionTarjetas _Frm = new Frm_VerificacionTarjetas();
                                if (!_Mtd_AbiertoOno(_Frm))
                                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                                else
                                { _Frm.Dispose(); }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Las tarjetas aún no han sido impresas. No podra realizar operaciones en este módulo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Las tarjetas aún no han sido creadas. No podra realizar operaciones en este módulo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                { MessageBox.Show("Su usuario no posee permiso para entrar en este módulo", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop); }

            }
        }
        private void verToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Busqueda2 _Frm = new Frm_Busqueda2(5);
                _Frm.Name = "FrmComparativo";
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void impreiónDeCódfigoDeBarrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ImprimeCodBar _Frm = new Frm_ImprimeCodBar();
                _Frm.Name = "FrmComparativo";
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void movimientoDeInventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsMovInventario _Frm = new Frm_ConsMovInventario();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void ajusteDeEntradaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                if (CLASES._Cls_Varios_Metodos._Mtd_Conteo_Iniciado())
                {
                    //if (myUtilidad._Mtd_UsuarioProceso(Frm_Padre._Str_Use, "F_CIERRE_CONTEO"))
                    //{
                    //    MessageBox.Show("Se ha iniciado el conteo de inventario.\n Si usted realiza operaciones en este módulo podria ocacionar descuadres en el inventario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    _Ctrl_Clave _Ctrl = new _Ctrl_Clave(1, this);
                    //}
                    //else
                    //{
                        //MessageBox.Show("Se ha iniciado el conteo de inventario.\n Usted no tiene permisos para realizar operaciónes en este ámbito", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                    MessageBox.Show("Se ha iniciado el conteo de inventario.\n No puede realizar operaciónes en este ámbito.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Frm_AjusteEntrada _Frm = new Frm_AjusteEntrada();
                    _Frm.Name = "Frm_AjusteEntrada";
                    if (!_Mtd_AbiertoOno(_Frm))
                    //{ _Frm.MdiParent = this; _Frm.Show(); }
                    { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                    else
                    { _Frm.Dispose(); }
                }
            }
        }
        private void ajusteDeSalidaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No puede trabajar en esta opción, por favor informar a CONSSA!!", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                if (CLASES._Cls_Varios_Metodos._Mtd_Conteo_Iniciado())
                {
                    //if (myUtilidad._Mtd_UsuarioProceso(Frm_Padre._Str_Use, "F_CIERRE_CONTEO"))
                    //{
                    //    MessageBox.Show("Se ha iniciado el conteo de inventario.\n Si usted realiza operaciones en este módulo podria ocacionar descuadres en el inventario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    _Ctrl_Clave _Ctrl = new _Ctrl_Clave(1, this);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Se ha iniciado el conteo de inventario.\n Usted no tiene permisos para realizar operaciónes en este ámbito", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                    MessageBox.Show("Se ha iniciado el conteo de inventario.\n No puede realizar operaciónes en este ámbito.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Frm_AjusteSalida _Frm = new Frm_AjusteSalida();
                    _Frm.Name = "Frm_AjusteSalida2";
                    if (!_Mtd_AbiertoOno(_Frm))
                    //{ _Frm.MdiParent = this; _Frm.Show(); }
                    { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                    else
                    { _Frm.Dispose(); }
                }
            }
        }

        private void bancoToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Banco _Frm = new Frm_Banco();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void _mnu_ConciliacionBancariaSaldoInicial_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConciliacionBancariaSaldoInicial _Frm = new Frm_ConciliacionBancariaSaldoInicial();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void capturaDeBancoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_CapturaBancoConfPos _Frm = new Frm_CapturaBancoConfPos();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void configuracionCapturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_CapturaDispBanco2 _Frm = new Frm_CapturaDispBanco2();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void conciliacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConciliacionBancariaV2 _Frm = new Frm_ConciliacionBancariaV2();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void _Mnu_Banco_CB_09_DisponibilidadBancaria_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                var _Frm = new Frm_Disponibilidad();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void Frm_Padre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                _Int_i = 1;
            }
        }
        private void Frm_Padre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_Int_i == 1)
            {
                switch (e.KeyChar)
                {
                    case (char)14:
                        if (CONTROLES._Ctrl_Buscar._Bl_Especial)
                        { _Ctrl_Buscar1._Bt_nuevo2.PerformClick(); }
                        else
                        { _Ctrl_Buscar1._Bt_nuevo.PerformClick(); }
                        break;
                    case (char)5:
                        if (CONTROLES._Ctrl_Buscar._Bl_Especial)
                        { _Ctrl_Buscar1._Bt_editar2.PerformClick(); }
                        else
                        { _Ctrl_Buscar1._Bt_editar.PerformClick(); }
                        break;
                    case (char)7:
                        if (CONTROLES._Ctrl_Buscar._Bl_Especial)
                        { _Ctrl_Buscar1._Bt_guardar2.PerformClick(); }
                        else
                        { _Ctrl_Buscar1._Bt_guardar.PerformClick(); }
                        break;
                    case (char)12:
                        if (CONTROLES._Ctrl_Buscar._Bl_Especial)
                        { _Ctrl_Buscar1._Bt_borrar2.PerformClick(); }
                        else
                        { _Ctrl_Buscar1._Bt_borrar.PerformClick(); }
                        break;
                }
            }
            _Int_i = 0;
        }
        private void Frm_Padre_KeyUp(object sender, KeyEventArgs e)
        {
            _Int_i = 0;
        }
        private void verificaciónDeRelaciónDeCobranzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                _Frm_Contenedor._Mtd_VerificarCierreCaja();
                if (!_Frm_Contenedor._Bol_CierreCajaActivado)
                {
                    string _Str_Url = CLASES._Cls_Varios_Metodos._Str_Servidor_Web + "mrelacionaprob.aspx?" + osio.encriptar("usuario") + "=" + osio.encriptar(Frm_Padre._Str_Use) + "&" + osio.encriptar("compania") + "=" + osio.encriptar(Frm_Padre._Str_Comp);
                    Frm_Navegador _Frm = new Frm_Navegador(_Str_Url, true);
                    _Frm.MdiParent = this;
                    _Frm.Dock = DockStyle.Fill;
                    _Frm.Show();
                }
                else
                {
                    MessageBox.Show("Este proceso esta bloqueado por el cierre de caja", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void toolStripMenuItem26_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_TabuladoresDespacho _Frm = new Frm_TabuladoresDespacho();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        private void cargaDeChequesDevueltosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                _Frm_Contenedor._Mtd_VerificarCierreCaja();
                if (!_Frm_Contenedor._Bol_CierreCajaActivado)
                {
                    Frm_IngCheqDevuelto _Frm = new Frm_IngCheqDevuelto();
                    if (!_Mtd_AbiertoOno(_Frm))
                    { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                    //{ _Frm.MdiParent = this; _Frm.Show(); }
                    else
                    { _Frm.Dispose(); }
                }
                else
                {
                    MessageBox.Show("Este proceso esta bloqueado por el cierre de caja", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                _Frm_Contenedor._Mtd_VerificarCierreCaja();
                if (!_Frm_Contenedor._Bol_CierreCajaActivado)
                {
                    Frm_EgreCheqTrans _Frm = new Frm_EgreCheqTrans();
                    if (!_Mtd_AbiertoOno(_Frm))
                    { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                    //{ _Frm.MdiParent = this; _Frm.Show(); }
                    else
                    { _Frm.Dispose(); }
                }
                else
                {
                    MessageBox.Show("Este proceso esta bloqueado por el cierre de caja", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void configuraciónImpresoraFacturaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void notaDeEntregaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_NotaEntrega _Frm = new Frm_NotaEntrega();
                if (!_Mtd_AbiertoOno(_Frm))
                {
                    _Frm.MdiParent = this; _Frm.Show();
                }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem34_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_OrdenCompra _Frm_G = new Frm_Inf_OrdenCompra();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void comprasPorProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_ComprasDetalladas _Frm_G = new Frm_Inf_ComprasDetalladas();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void cxPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_CxP _Frm_G = new Frm_Inf_CxP();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void ordenesDePagoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_OrdenPago _Frm_G = new Frm_Inf_OrdenPago();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void relaciónDeNDCxPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_ND_CxP _Frm_G = new Frm_Inf_ND_CxP();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void relaciónDeNCCxCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_NC_CxP _Frm_G = new Frm_Inf_NC_CxP();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void relaciónDeNDAnuladasCxPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_NDA_CxP _Frm_G = new Frm_Inf_NDA_CxP();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void relaciónDeNCAnuladasCxPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_NCA_CxP _Frm_G = new Frm_Inf_NCA_CxP();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void precargaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_PreCarga _Frm_G = new Frm_Inf_PreCarga();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void facturasPorDespacharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_FacturaDespachar _Frm_G = new Frm_Inf_FacturaDespachar();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }



        private void toolStripMenuItem39_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                //Frm_ConsultaFactura _Frm_G = new Frm_ConsultaFactura();
                Frm_ConsultaMultiple _Frm_G = new Frm_ConsultaMultiple(_Enu_TiposConsultas.Factura);

                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void toolStripMenuItem35_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_ComprasResumidas _Frm_G = new Frm_Inf_ComprasResumidas();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void impresorasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConfigPrinterFact _Frm = new Frm_ConfigPrinterFact();
                if (!_Mtd_AbiertoOno(_Frm))
                {
                    this.Cursor = Cursors.WaitCursor;
                    _Frm.MdiParent = this; _Frm.Show();
                    this.Cursor = Cursors.Default;
                }
                else
                { _Frm.Dispose(); }
            }
        }


        private void menúDelUsuarioToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void configuraciónCOMPRASToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConfigCompra _Frm = new Frm_ConfigCompra();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
            
        }

        private void cTASPORPAGARToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConfigCxP _Frm = new Frm_ConfigCxP();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void ctasPorCobrarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConfigCredito _Frm = new Frm_ConfigCredito();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void devolucionesEnVentaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_DevolVenta _Frm = new Frm_DevolVenta();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void notaDeRecepciónToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_NotaRecepcion _Frm = new Frm_NotaRecepcion();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void almacénYDespachoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConfigInvent _Frm = new Frm_ConfigInvent();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void tabsPorUsuarioToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            
        }

        private void toolStripMenuItem46_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem41_Click(object sender, EventArgs e)
        {            
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Compania _Frm = new Frm_Compania();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void ventaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConfigVentas _Frm = new Frm_ConfigVentas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void bancoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConfigBanco _Frm = new Frm_ConfigBanco();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem42_Click(object sender, EventArgs e)
        {

        }

        private void valorizadoDeInventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_ValorizaInventario _Frm = new Frm_Inf_ValorizaInventario();
                if (!_Mtd_AbiertoOno(_Frm))
                //{ _Frm.MdiParent = this; _Frm.Show(); }
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void costoYUtilidadDeProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            //if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            //{
            //    Frm_Inf_CostoUtilProducto _Frm = new Frm_Inf_CostoUtilProducto();
            //    if (!_Mtd_AbiertoOno(_Frm))                
            //    { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
            //    else
            //    { _Frm.Dispose(); }
            //}

            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaMultiple _Frm = new Frm_ConsultaMultiple(_Enu_TiposConsultas.CostoUtilidadProducto);

                if (!_Mtd_AbiertoOno(_Frm))
                {
                    _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show();
                }
                else
                {
                    _Frm.Dispose();
                }
            }
        }

        private void analisisDeSaldoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            


        }

        private void devolucionesEnVentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ////MessageBox.Show("No puede trabajar en esta opción, por favor informar a CONSSA!!", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_DevolVenta _Frm = new Frm_DevolVenta();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void relaciónDeFacturasEmitidasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_Facturas _Frm = new Frm_Inf_Facturas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void ventasAcumuladaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No puede trabajar en esta opción, por favor informar a CONSSA!!", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_AcumVenta _Frm = new Frm_Inf_AcumVenta();
                if (!_Mtd_AbiertoOno(_Frm))
                //{ _Frm.MdiParent = this; _Frm.Show(); }
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void anulaciónDeFacturasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ////MessageBox.Show("No puede trabajar en esta opción, por favor informar a CONSSA!!", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
           if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_AnulacionFactura _Frm = new Frm_AnulacionFactura();
                if (!_Mtd_AbiertoOno(_Frm))
                //{ _Frm.MdiParent = this; _Frm.Show(); }
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem24_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No puede trabajar en esta opción, esta en mantenimiento!!", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_RelaFacturaCobro _Frm = new Frm_RelaFacturaCobro();
                if (!_Mtd_AbiertoOno(_Frm))
                //{ _Frm.MdiParent = this; _Frm.Show(); }
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void factorDeCobranzaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Frm_FactorCuotaCob _Frm = new Frm_FactorCuotaCob();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
        }

        private void cuotasToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Frm_CuotaCobranza _Frm = new Frm_CuotaCobranza();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
        }

        private void pruebaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_PrintVarios _Frm = new Frm_PrintVarios();
            if (!_Mtd_AbiertoOno(_Frm))
            { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
            else
            { _Frm.Dispose(); }
        }

        private void trasladoDeDocumentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No puede trabajar en esta opción, por favor informar a CONSSA!!", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (myUtilidad._Mtd_UsuarioProceso(Frm_Padre._Str_Use, "F_TRASCART_CXC"))
            {
                Frm_TrasladoDocumentos _Frm = new Frm_TrasladoDocumentos();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
            else
            {
                MessageBox.Show("Este usuario no tiene permiso para accesar a esta opción.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void trasladoDeDocumentosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No puede trabajar en esta opción, por favor informar a CONSSA!!", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (myUtilidad._Mtd_UsuarioProceso(Frm_Padre._Str_Use, "F_TRASCART_CXC"))
            {
                Frm_TrasladoCartera _Frm = new Frm_TrasladoCartera();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
            else
            {
                MessageBox.Show("Este usuario no tiene permiso para accesar a esta opción.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void analisisDeSaldoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_PlanCuentas _Frm = new Frm_Inf_PlanCuentas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
            
        }

        private void analisisDeSaldoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_AnalisisSaldo _Frm = new Frm_Inf_AnalisisSaldo();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void Frm_Padre_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                statusBar1.Panels[4].Text = this.ActiveMdiChild.Name;
            }
        }

        private void toolStripMenuItem49_Click(object sender, EventArgs e)
        {
            
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_AuxContable _Frm = new Frm_AuxContable();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            ////MessageBox.Show("No puede trabajar en esta opción, por favor informar a CONSSA!!", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_DevolVenta _Frm = new Frm_DevolVenta();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem50_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Clientes_VC_1 _Frm = new Frm_Clientes_VC_1();
                //_Frm.Name = "Frm7";
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void acumuladoDeCobranzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_AcumCob _Frm = new Frm_Inf_AcumCob();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void prueba3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Prospectos _Frm = new Frm_Prospectos();
            if (!_Mtd_AbiertoOno(_Frm))
            { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
            else
            { _Frm.Dispose(); }
        }

        private void _Ctrl_Buscar1_Load(object sender, EventArgs e)
        {

        }

        private void relaciónDeFacturasAnuladasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_FacturasAnul _Frm = new Frm_Inf_FacturasAnul();
                if (!_Mtd_AbiertoOno(_Frm))                
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void notasCréditoAnuladasToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void nDAnuladasClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
         
        }

        private void relaciónDeNDEmitidasClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
        }

        private void relaciónDeChequesDevueltosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void relaciónDeIngresoDeChequesDevueltosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void relaciónDeClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_ClienteZonaRuta _Frm = new Frm_Inf_ClienteZonaRuta();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        //cambio
        private void cargaDeComprobantesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ComprobanteContable _Frm = new Frm_ComprobanteContable();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void mayorAnalíticoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_MayorAnalitico _Frm = new Frm_Inf_MayorAnalitico();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void utilidadPorLíneaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_UtilidadProvee _Frm = new Frm_Inf_UtilidadProvee();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void acumuladoDeVentasPorGerenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_AcumVtaGerArea _Frm = new Frm_Inf_AcumVtaGerArea();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem62_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem65_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_OrdenPago _Frm = new Frm_OrdenPago(true);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void reimpresionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myUtilidad._Mtd_UsuarioProceso(Frm_Padre._Str_Use, "F_REIMPREFACT"))
            {
                Frm_ReImpresionFacturas _Frm = new Frm_ReImpresionFacturas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
            else
            {
                MessageBox.Show("Usted no tiene permisos para ingresar a este módulo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            
        }

        private void librosDeCompraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_LibroCompras _Frm = new Frm_Inf_LibroCompras();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
            
        }

        private void libroDeVentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_LibroVentas _Frm = new Frm_Inf_LibroVentas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void descuentosMalOtorgadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            //{
            //    if (myUtilidad._Mtd_UsuarioProceso(Frm_Padre._Str_Use, "F_N1_ANULFACTURACXP") | myUtilidad._Mtd_UsuarioProceso(Frm_Padre._Str_Use, "F_N2_ANULFACTURACXP"))
            //    {
            //        Frm_AnulacionFacturaCxP _Frm = new Frm_AnulacionFacturaCxP();
            //        if (!_Mtd_AbiertoOno(_Frm))
            //        { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
            //        else
            //        { _Frm.Dispose(); }
            //    }
            //    else
            //    { MessageBox.Show("Usted no tiene permisos para acceder a este módulo", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
            //}

            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_DescMalOtorgados _Frm = new Frm_Inf_DescMalOtorgados();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem67_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_AnulacionFacturaCxP _Frm = new Frm_AnulacionFacturaCxP("FACT");
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void documentosEnPoderDelVendedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
             if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_DocEnPoderVen _Frm = new Frm_Inf_DocEnPoderVen();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void documentosVencidosAUnPlazoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_DocVencPla _Frm = new Frm_Inf_DocVencPla();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void relaChequesRecuperadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
 
        }

        private void ventasPorProductoYTipoDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_VentProdTipClien _Frm = new Frm_Inf_VentProdTipClien();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void fichaDelClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_FichaCliente _Frm = new Frm_Inf_FichaCliente();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem23_Click_1(object sender, EventArgs e)
        {

        }

        private void relaciónDeNCEmitidasClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void calculoDeCuotaDeCobranzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                        if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_CuotasCob _Frm = new Frm_CuotasCob();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }  
        }

        private void relaciónDeChequesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_List_CheqTransm _Frm = new Frm_Inf_List_CheqTransm();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            } 
        }

        private void rotaciónDeInventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_RotInv _Frm = new Frm_Inf_RotInv();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            } 
        }

        private void ventasPorProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_VentasAcumProdvend _Frm = new Frm_Inf_VentasAcumProdvend();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            } 

        }

        private void movimientoDeInventarioAjustesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                                      if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_MovInvAjuste _Frm = new Frm_Inf_MovInvAjuste();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            } 
        }

        private void balanceDeComprobaciómToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //prueba
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_EstadoGananyPerd _Frm = new Frm_Inf_EstadoGananyPerd();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void consultaDeComprobantesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaComprobante _Frm = new Frm_ConsultaComprobante();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
            
        }

        private void rutaDeVisitaVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_RutaVisitaVend _Frm = new Frm_Inf_RutaVisitaVend();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            } 
        }

        private void frmInfGestionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void analisisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_AnalisisCompInv _Frm = new Frm_Inf_AnalisisCompInv();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void listadoDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_Productos _Frm = new Frm_Inf_Productos();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void pruebaToolStripMenuItem1_Click(object sender, EventArgs e)
        {           

                                if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                PANTALLA _Frm = new PANTALLA();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void informaciónDePedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
                                if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_Pedidos _Frm = new Frm_Inf_Pedidos();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void análisisDeCompraActualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_AnalisisCompInv _Frm = new Frm_Inf_AnalisisCompInv();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void ventasToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void informeListadoDeClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_ConsultaClientes _Frm = new Frm_Inf_ConsultaClientes();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void informeHistorialDelClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {

               if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_HistCliente _Frm = new Frm_Inf_HistCliente();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void historicoDeInventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_HistInventario _Frm = new Frm_Inf_HistInventario();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void reporteDeListaDePrecioToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_ListadPrecio _Frm = new Frm_Inf_ListadPrecio();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void acumuladoDeVentasPorClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_AcumVtasCliente _Frm = new Frm_Inf_AcumVtasCliente();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem88_Click(object sender, EventArgs e)
        {

            
        }

        private void toolStripMenuItem90_Click(object sender, EventArgs e)
        {

        }

        private void ctasPorCobrarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem91_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem92_Click(object sender, EventArgs e)
        {

        }

        private void notificacionesPorUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Tabs _Frm = new Frm_Tabs();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void menúDelUsuarioToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Permisos prueba = new Frm_Permisos();
                if (!_Mtd_AbiertoOno(prueba))
                { prueba.MdiParent = this; prueba.Show(); }
                else
                { prueba.Dispose(); }
            }
        }

        private void cobranzaDetalladaCajasCerradasToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void nCEmitidasCajasCerradasToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void nCAplicadasCajasCerradasToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cuadreDeCajasCerradasToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cobranzaDetalladaCajasCerradasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_CobranDetalladaCaja _Frm = new Frm_Inf_CobranDetalladaCaja();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void nCEmitidasCajasCerradasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_NC_Emit_Caja _Frm = new Frm_Inf_NC_Emit_Caja();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void nCAplicadasCajasCerradasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_NC_Aplic_Caja _Frm = new Frm_Inf_NC_Aplic_Caja();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void cuadreDeCajasCerradasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_CuadreCaja _Frm = new Frm_Inf_CuadreCaja();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void relaciónDeNCEmitidasClienteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_NC_Emit _Frm = new Frm_Inf_NC_Emit();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }   
        }

        private void relaciónDeNCAnuladasClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_NC_Anul _Frm = new Frm_Inf_NC_Anul();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem45_Click(object sender, EventArgs e)
        {

        }

        private void relaciónDeNDEmitidasClienteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_ND_Emit _Frm = new Frm_Inf_ND_Emit();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }   
        }

        private void relaciónDeNDAnuladasClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_ND_Anul _Frm = new Frm_Inf_ND_Anul();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void relaciónDeChequesDevueltosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_CheqDev _Frm = new Frm_Inf_CheqDev();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void relaciónIngresoDeChequesDevueltosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_IngCheqDev _Frm = new Frm_Inf_IngCheqDev();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void relaciónDeChequesEnTránsitoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_EgreCheqTransito _Frm = new Frm_Inf_EgreCheqTransito();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void relaciónChequesRecuperadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_RelaCheqRecu _Frm = new Frm_Inf_RelaCheqRecu();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem57_Click(object sender, EventArgs e)
        {

            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_Gestion _Frm = new Frm_Inf_Gestion();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void listadoDeRetencionesDeIVASegúnUnaCajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_LisRetenIva_Caja _Frm = new Frm_Inf_LisRetenIva_Caja();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem58_Click(object sender, EventArgs e)
        {
                        if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaCompra _Frm = new Frm_ConsultaCompra();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
            
        }

        private void toolStripMenuItem62_Click_1(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ReAbrirMesCont _Frm = new Frm_ReAbrirMesCont();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void informeDeRelaciónDeDespachoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_RelaDespacho _Frm = new Frm_Inf_RelaDespacho();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void indiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_IndicDespacho _Frm = new Frm_Inf_IndicDespacho();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem82_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ControlChequeras _Frm = new Frm_ControlChequeras();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                    //{ _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem90_Click_1(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_AcumCobDia _Frm = new Frm_Inf_AcumCobDia();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                //{ _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void contenidoDeLaAyudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process _System_Proceso = System.Diagnostics.Process.Start("http://192.168.1.5/t3wiki/index.php/Tabla_de_contenido");
            }
            catch { }
        }

        private void últimasActualizacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process _System_Proceso = System.Diagnostics.Process.Start("http://192.168.1.5/t3wiki/index.php/T3Wiki:Actualidad");
            }
            catch { }
        }

        private void acerrcaDeT3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process _System_Proceso = System.Diagnostics.Process.Start("http://192.168.1.5/t3wiki/index.php/T3");
            }
            catch { }
        }

        private void toolStripMenuItem23_Click_2(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_AnulaChequeEmitido _Frm = new Frm_AnulaChequeEmitido();
                _Frm.Name = "Frm_AnulaChequeEmitido_Menu";
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem44_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem93_Click(object sender, EventArgs e)
        {
            
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaAuxiliarCont _Frm = new Frm_ConsultaAuxiliarCont();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }            
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem94_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_PreOrden _Frm = new Frm_PreOrden();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem95_Click(object sender, EventArgs e)
        {
                        if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaPreOrden _Frm = new Frm_ConsultaPreOrden();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
            
        }

        private void librosLegalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_LibrosLegales _Frm = new Frm_Inf_LibrosLegales();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem94_Click_1(object sender, EventArgs e)
        {
             if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_UsuarioAdminReseteo _Frm = new Frm_UsuarioAdminReseteo();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void archivosSENIATToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                          if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ArchivoSeniat _Frm = new Frm_ArchivoSeniat();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }


        private void toolStripMenuItem98_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_HistVendedor _Frm_G = new Frm_HistVendedor();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void toolStripMenuItem99_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_HistCliente _Frm_G = new Frm_HistCliente();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void clientesProspectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Prospectos _Frm = new Frm_Prospectos();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem102_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_Varios _Frm = new Frm_Inf_Varios(8, _Str_GroupComp, "", DateTime.Now.ToShortDateString());
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void informeSICAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_SICA _Frm = new Frm_Inf_SICA();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem103_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_GuiaDespacho _Frm = new Frm_Inf_GuiaDespacho();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem104_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_GuiaConDevol _Frm = new Frm_Inf_GuiaConDevol();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem105_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_ListFactEmit _Frm = new Frm_Inf_ListFactEmit();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem106_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_RelaFactCobro _Frm = new Frm_Inf_RelaFactCobro();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void chequesEnTransitoIngresadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_IngCheqTrans_Caja _Frm_G = new Frm_Inf_IngCheqTrans_Caja();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void toolStripMenuItem107_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_EgreCheqTrans_Caja _Frm_G = new Frm_Inf_EgreCheqTrans_Caja();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void almacénToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem108_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ComprobISLRManual _Frm = new Frm_ComprobISLRManual();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void reposicionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ReposicionCxP _Frm = new Frm_ReposicionCxP();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem109_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_BalanceGeneral _Frm = new Frm_Inf_BalanceGeneral();
                if (!_Mtd_AbiertoOno (_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }               
            }
        }

        private void balanceDeComprobaciómToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_BalanceComprob _Frm = new Frm_Inf_BalanceComprob();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void informesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void CorreccionNumCtrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_NumControl _Frm = new Frm_NumControl();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_ReporteUniticket_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Uniticket _Frm = new Frm_Uniticket();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Informe_DescuentosFinancieros_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_DescFinancieros _Frm = new Frm_Inf_DescFinancieros();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Tm_TiempoUserRestric_Tick(object sender, EventArgs e)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(_async_Default);
            }
            catch
            {
            }
        }

        private void toolStripMenuItem112_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_UsuariosEntSalLog _Frm = new Frm_Inf_UsuariosEntSalLog();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        
        // -- reportes incentivos de ventas
        
        private void _Mnu_Inf_IV_parametros_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_IncParams _Frm = new Frm_Inf_IncParams();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_IV_cobranza_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_IncCobranza _Frm = new Frm_Inf_IncCobranza();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_IV_distribucion_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_IncDistribucion _Frm = new Frm_Inf_IncDistribucion();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_IV_marcafoco_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_IncMF _Frm = new Frm_Inf_IncMF();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_IV_surtidoideal_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_IncSI _Frm = new Frm_Inf_IncSI();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_IV_varios_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_IncVarios _Frm = new Frm_Inf_IncVarios();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_IV_visibilidad_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_IncVisibilidad _Frm = new Frm_Inf_IncVisibilidad();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_IV_volumen_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_IncVolVentas _Frm = new Frm_Inf_IncVolVentas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_IV_consolidado_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_IncConsolidado _Frm = new Frm_Inf_IncConsolidado();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        
        // -- config incentivos de ventas
        private void _Mnu_IV_grupos_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_IncGrupos _Frm = new Frm_IncGrupos();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_IV_cobranza_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_IncCobranza _Frm = new Frm_IncCobranza();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_IV_distribucion_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_IncDistribucion _Frm = new Frm_IncDistribucion();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_IV_marcafoco_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_IncMarcaFoco _Frm = new Frm_IncMarcaFoco();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_IV_varios_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_IncVarios _Frm = new Frm_IncVarios();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_IV_visibilidad_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_IncVisibilidad _Frm = new Frm_IncVisibilidad();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.None; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_IV_volumen_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_IncVentas _Frm = new Frm_IncVentas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_IV_exportacionSPI_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ExportarIncSPI _Frm = new Frm_ExportarIncSPI();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.None; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        // --- hasta aqui incentivos de venta ========================================================================

        private void _Mnu_Inf_ClientesAPD_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_Varios _Frm = new Frm_Inf_Varios(10, _Str_GroupComp);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }

        }

        private void toolStripMenuItem113_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_FechaVencimientoCapas _Frm = new Frm_Inf_FechaVencimientoCapas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem114_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_AnulacionFacturaCxP _Frm = new Frm_AnulacionFacturaCxP("NDP");
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem115_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_AnulacionFacturaCxP _Frm = new Frm_AnulacionFacturaCxP("NCP");
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_CostoUtilidadVendedor_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_UtilidadVendedor _Frm = new Frm_Inf_UtilidadVendedor();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }
        
        private void toolStripMenuItem116_Click_1(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_InventarioSica _Frm = new Frm_Inf_InventarioSica();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_DevolCompSica_Click(object sender, EventArgs e)
        {

        }

        private void _Mnu_Inf_AnalisisClientes_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_AnalisisClientes _Frm = new Frm_Inf_AnalisisClientes();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_NotaRecepLote_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_NotaRecepLote _Frm = new Frm_Inf_NotaRecepLote();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_TipoDeOperacionBancariaSegunBanco_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_OperBancD _Frm = new Frm_OperBancD();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_TipoDeOperacionBancariaSegunBancoParaNoCapturar_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_OperBancDExcep _Frm = new Frm_OperBancDExcep();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_TipoDeOperacionBancaria_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_OperBanc _Frm = new Frm_OperBanc();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_InventarioPorCapasLote_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_InvPorLotes _Frm = new Frm_InvPorLotes();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_EfectEntrega_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_EfectEntrega _Frm = new Frm_Inf_EfectEntrega();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_ReporteActivacionDeClientes_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {

                // id=1 Reporte de activación de clientes
                // id=2 Reporte de activación de clientes por grupo
                // id=3 Reporte de activación de clientes por tipo de establecimiento

                // este es el URL al que redireccion el RECEPTOR de T3Web
                string _Str_URLParametroEncriptado = osio.encriptar("Frm_Reportes2.aspx?id=1&omitirmenu=1");
                string _Str_CCOMPANYEncriptado = osio.encriptar(Frm_Padre._Str_Comp);
                string _Str_CUSEREncriptado = osio.encriptar(Frm_Padre._Str_Use);
                string _Str_Url = CLASES._Cls_Varios_Metodos._Str_Servidor_Web_2 + "Frm_ReceptorWin.aspx?Y0dGeVlXMWxkSEp2TVErQUQwQVBRLQSGvSGv=" + _Str_URLParametroEncriptado + "&Y0dGeVlXMWxkSEp2TWcrQUQwQVBRLQSGvSGv=" + _Str_CCOMPANYEncriptado + "&Y0dGeVlXMWxkSEp2TXcrQUQwQVBRLQSGvSGv=" + _Str_CUSEREncriptado;

                Frm_Navegador _Frm = new Frm_Navegador(_Str_Url, true);
                _Frm.MdiParent = this;
                _Frm.Dock = DockStyle.Fill;
                _Frm.Show();
            }
        }

        private void _Mnu_ReporteActivacionDeClientesPorGrupo_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {

                // id=1 Reporte de activación de clientes
                // id=2 Reporte de activación de clientes por grupo
                // id=3 Reporte de activación de clientes por tipo de establecimiento

                // este es el URL al que redireccion el RECEPTOR de T3Web
                string _Str_URLParametroEncriptado = osio.encriptar("Frm_Reportes2.aspx?id=2&omitirmenu=1");
                string _Str_CCOMPANYEncriptado = osio.encriptar(Frm_Padre._Str_Comp);
                string _Str_CUSEREncriptado = osio.encriptar(Frm_Padre._Str_Use);
                string _Str_Url = CLASES._Cls_Varios_Metodos._Str_Servidor_Web_2 + "Frm_ReceptorWin.aspx?Y0dGeVlXMWxkSEp2TVErQUQwQVBRLQSGvSGv=" + _Str_URLParametroEncriptado + "&Y0dGeVlXMWxkSEp2TWcrQUQwQVBRLQSGvSGv=" + _Str_CCOMPANYEncriptado + "&Y0dGeVlXMWxkSEp2TXcrQUQwQVBRLQSGvSGv=" + _Str_CUSEREncriptado;

                Frm_Navegador _Frm = new Frm_Navegador(_Str_Url, true);
                _Frm.MdiParent = this;
                _Frm.Dock = DockStyle.Fill;
                _Frm.Show();
            }

        }

        private void _Mnu_ReporteActivacionDeClientesPorTipoEstablecimiento_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {

                // id=1 Reporte de activación de clientes
                // id=2 Reporte de activación de clientes por grupo
                // id=3 Reporte de activación de clientes por tipo de establecimiento

                // este es el URL al que redireccion el RECEPTOR de T3Web
                string _Str_URLParametroEncriptado = osio.encriptar("Frm_Reportes2.aspx?id=3&omitirmenu=1");
                string _Str_CCOMPANYEncriptado = osio.encriptar(Frm_Padre._Str_Comp);
                string _Str_CUSEREncriptado = osio.encriptar(Frm_Padre._Str_Use);
                string _Str_Url = CLASES._Cls_Varios_Metodos._Str_Servidor_Web_2 + "Frm_ReceptorWin.aspx?Y0dGeVlXMWxkSEp2TVErQUQwQVBRLQSGvSGv=" + _Str_URLParametroEncriptado + "&Y0dGeVlXMWxkSEp2TWcrQUQwQVBRLQSGvSGv=" + _Str_CCOMPANYEncriptado + "&Y0dGeVlXMWxkSEp2TXcrQUQwQVBRLQSGvSGv=" + _Str_CUSEREncriptado;

                Frm_Navegador _Frm = new Frm_Navegador(_Str_Url, true);
                _Frm.MdiParent = this;
                _Frm.Dock = DockStyle.Fill;
                _Frm.Show();
            }

        }

        private void avisoDeCobroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_AvisoCobro _Frm = new Frm_AvisoCobro(0,0);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void anulaciónDeAvisoDeCobroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_AvisoCobro _Frm = new Frm_AvisoCobro(1, 0);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void avisoDePagoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_AvisoPago _Frm = new Frm_AvisoPago(0, 0);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void anulaciónDeAvisoDePagoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_AvisoPago _Frm = new Frm_AvisoPago(1, 0);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_ConsolidadoIntercompanias_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsolidadoIntercomp _Frm = new Frm_ConsolidadoIntercomp();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            } 
        }

        private void  _Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaMultiple _Frm = new Frm_ConsultaMultiple(_Enu_TiposConsultas.RecepcionComprasResumidoSemanal);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            } 

        }

        private void toolStripMenuItem121_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_PreindicacionCheques _Frm = new Frm_PreindicacionCheques();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            } 
        }

        private void toolStripMenuItem122_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_PreIndCheques _Frm = new Frm_Inf_PreIndCheques();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            } 
        }

        private void toolStripMenuItem123_Click(object sender, EventArgs e)
        {
             if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ClientesSinSubClasificacionITT _Frm = new Frm_ClientesSinSubClasificacionITT();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }              
        }

        private void _Mnu_Inf_NotaRecepcionDetallado_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaMultiple _Frm = new Frm_ConsultaMultiple(_Enu_TiposConsultas.NotasRecepcionDetallado);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            } 
        }

        private void _Mnu_Inf_DetalleLotesFacturas_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_DetalleLoteFact _Frm = new Frm_Inf_DetalleLoteFact();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_EfectividadCobranza_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaMultiple _Frm = new Frm_ConsultaMultiple(_Enu_TiposConsultas.EfectividadCobranza);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_ReporteClientesPorEstado_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaMultiple _Frm = new Frm_ConsultaMultiple(_Enu_TiposConsultas.ClientesEstatus);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_IV_surtidoideal_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_IncSI _Frm = new Frm_IncSI();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Estratificacion_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Estratificacion _Frm = new Frm_Estratificacion();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_ChequesTransitoDepositados_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaMultiple _Frm = new Frm_ConsultaMultiple(_Enu_TiposConsultas.RelacionChequesTransitoDepositado);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem124_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Beneficiario _Frm = new Frm_Beneficiario();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem125_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaMultiple _Frm = new Frm_ConsultaMultiple(_Enu_TiposConsultas.ValorizadoInventario);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem126_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_EALCCConfig _Frm = new Frm_EALCCConfig();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem127_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaMultiple _Frm = new Frm_ConsultaMultiple(_Enu_TiposConsultas.LimiteCreditoClientes);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_RelacionProveedorCliente_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_IC_RelaProvCli _Frm = new Frm_IC_RelaProvCli();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_CobranzasIC_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_IC_Cobranza _Frm = new Frm_IC_Cobranza();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_VentasDetporFactPmv_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsultaMultiple _Frm = new Frm_ConsultaMultiple(_Enu_TiposConsultas.FacturasProductos);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void mnuConsInterRes_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConsolidadoInterResumido _Frm = new Frm_ConsolidadoInterResumido();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void mnuMantenimientoMAAC_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_MantenimientoMAAC _Frm = new Frm_MantenimientoMAAC();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItemConfiguracionImpresion_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ConfigImpresion _Frm = new Frm_ConfigImpresion();
                if (!_Mtd_AbiertoOno(_Frm))
                {
                    this.Cursor = Cursors.WaitCursor;
                    _Frm.MdiParent = this; _Frm.Show();
                    this.Cursor = Cursors.Default;
                }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_PrefSegPrec_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_PrefSegPrec _Frm = new Frm_Inf_PrefSegPrec();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_CompRetenIvaCxC_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                var _Frm = new Frm_ComprobRetenCxC();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem101_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_InvCapas _Frm = new Frm_Inf_InvCapas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void reimpresionPruebasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myUtilidad._Mtd_UsuarioProceso(Frm_Padre._Str_Use, "F_REIMPREFACT"))
            {
                Frm_ReImpresionFacturasPruebas _Frm = new Frm_ReImpresionFacturasPruebas();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
            else
            {
                MessageBox.Show("Usted no tiene permisos para ingresar a este módulo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void _Mnu_Inf_ListadoProveedores_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_Proveedores _Frm_G = new Frm_Inf_Proveedores();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void _Mnu_Inf_RelacPorCaja_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_RelacPorCaja _Frm_G = new Frm_Inf_RelacPorCaja();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void toolStripMenuItem118_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ReimpresionCobros _Frm_G = new Frm_ReimpresionCobros();
                if (!_Mtd_AbiertoOno(_Frm_G))
                { _Frm_G.MdiParent = this; _Frm_G.Dock = DockStyle.Fill; _Frm_G.Show(); }
                else
                { _Frm_G.Dispose(); }
            }
        }

        private void _Mnu_CompRetenIvaManual_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                var _Frm = new Frm_CompRetencionManual();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_EdicionEstatusGuia_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                var _Frm = new Frm_GuiaManual();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Banco_CB_08_IniciaConciliacion_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_InicializacionConciliacion _Frm = new Frm_InicializacionConciliacion();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_InvCompromPreFact_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_InvCompromPreFact _Frm = new Frm_Inf_InvCompromPreFact();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_Inf_Prefacturas_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_PreFactResProv _Frm = new Frm_Inf_PreFactResProv();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_AjusteIntegrado_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No puede trabajar en esta opción, por favor informar a CONSSA!!", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                if (CLASES._Cls_Varios_Metodos._Mtd_Conteo_Iniciado())
                {
                    //if (myUtilidad._Mtd_UsuarioProceso(Frm_Padre._Str_Use, "F_CIERRE_CONTEO"))
                    //{
                    //    MessageBox.Show("Se ha iniciado el conteo de inventario.\n Si usted realiza operaciones en este módulo podria ocacionar descuadres en el inventario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    _Ctrl_Clave _Ctrl = new _Ctrl_Clave(1, this);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Se ha iniciado el conteo de inventario.\n Usted no tiene permisos para realizar operaciónes en este ámbito", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                    MessageBox.Show("Se ha iniciado el conteo de inventario.\n No puede realizar operaciónes en este ámbito.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Frm_AjusteIntegrado _Frm = new Frm_AjusteIntegrado(0);
                    if (!_Mtd_AbiertoOno(_Frm))
                    //{ _Frm.MdiParent = this; _Frm.Show(); }
                    { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                    else
                    { _Frm.Dispose(); }
                }
            }
        }

        private void _Mnu_ProductosMargenExcedido_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_MargenExcedido _Frm = new Frm_Inf_MargenExcedido();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void diasConciliadosYNoConciliadosMenuItem_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_Inf_DiasConciliados _Frm = new Frm_Inf_DiasConciliados();
                if (!_Mtd_AbiertoOno(_Frm))
                //{ _Frm.MdiParent = this; _Frm.Show(); }
                { _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void _Mnu_ComplementoLibroCompras_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ComplementoLibroCompras _Frm = new Frm_ComplementoLibroCompras(false);
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void toolStripMenuItem118_Click_1(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ComprobIAEManual _Frm = new Frm_ComprobIAEManual();
                if (!_Mtd_AbiertoOno(_Frm))
                { _Frm.MdiParent = this; _Frm.Show(); }
                else
                { _Frm.Dispose(); }
            }
        }

        private void Import_Nomina_ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CLASES._Cls_Varios_Metodos._Mtd_VerificarCnn(this))
            {
                Frm_ImportNomina _Frm = new Frm_ImportNomina();
                if (!_Mtd_AbiertoOno(_Frm))
                {
                    _Frm.MdiParent = this; _Frm.Dock = DockStyle.Fill; _Frm.Show();
                }
                else
                { _Frm.Dispose(); }
            }
        }
       
    }
}