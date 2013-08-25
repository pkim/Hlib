/*
 * Name: ProxyDialog
 * Date: 08 Jänner 2011
 * Author: Patrik Kimmeswenger
 * Description: Serves the Graphical User Interface (GUI) to set the parameters of the Proxy
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Handler;
using System.IO;
using FileHandler.XML;
using NetworkHandler.IP.Proxy;


namespace FormsGUIHandler.Settings
{

    /// <summary>
    /// Serves the Graphical User Interface (GUI) to set the parameters of the Proxy
    /// </summary>
    public partial class ProxyDialog : Form
    {

        #region Items

        // Proxy
        private Proxy    proxy;
        private WebProxy webProxy;

        // Serialization
        private String  serialization_DirectoryPath = Application.StartupPath;   // default value
        private String  serialization_filename      = "proxy.xml";               // default value
        private Boolean serialization = false;

        #endregion


        #region Properties


        // WebProxy
        public WebProxy WebProxy { 

            get 
            {
                return this.webProxy;
            }
        }

        // Serialization
        public Boolean Serialization {

            get
            {
                return this.serialization;
            }

            set
            {
                if (value == true)
                {             
                    this.deserialize_Proxy();
                    Console.WriteLine("hier");
                }

                this.serialization = value;
            }
        }

        public String Serialization_DirectoryPath
        {
            get
            {
                return this.serialization_DirectoryPath;
            }

            set
            {
                if(Directory.Exists(value))
                {
                    this.serialization_DirectoryPath = value;
                }

                else throw new IOException("Directory " + value + " doesn´t exist");
            }
        }

        public String Serialization_Filename
        {
            get
            {
                return this.serialization_filename;
            }

            set
            {
                 this.serialization_filename = value;
            }
        }

        #endregion


        #region Constructor

        public ProxyDialog()
        {
            InitializeComponent();

            // Proxy
            
            this.proxy    = new Proxy();
            this.webProxy = null;

            //Serialization
            this.Serialization = false;

        }

        #endregion


        #region Events

        private void checkBox_EnbaleProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_EnbaleProxy.Checked)
            {
                this.groupBox_ProxySettings.Enabled = true;
                this.proxy.Enabled = true;
            }

            else
            {
                this.groupBox_ProxySettings.Enabled = false;
                this.proxy.Enabled = false;
            }
        }

        private void textBox_WebProxy_TextChanged(object sender, EventArgs e)
        {
            this.proxy.WebProxyAdress = this.textBox_WebProxy.Text;
        }

        private void textBox_ProxyUsername_TextChanged(object sender, EventArgs e)
        {
            this.proxy.Username = this.textBox_ProxyUsername.Text;
        }

        private void textBox_ProxyPassword_TextChanged(object sender, EventArgs e)
        {
            this.proxy.Password = this.textBox_ProxyPassword.Text;
        }

        private void numericUpDown_Port_ValueChanged(object sender, EventArgs e)
        {
            this.proxy.Port = Convert.ToInt32(this.numericUpDown_Port.Value); 
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Visible = false;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            // GetWebProxy
            this.webProxy = this.proxy.getWebProxy();

            // XML Serialization
            if (this.Serialization)
            {
                if (this.serialize_Proxy()) // if successfull
                {
                    this.Visible = false;
                    this.DialogResult = DialogResult.OK;
                }
            }

        }

        private void ProxyDialog_Load(object sender, EventArgs e)
        {

            // Serialization is enabled
            if (this.Serialization) 
            {
                this.deserialize_Proxy();
            }
                
        }

        #endregion


        #region Methods

        /// <summary>
        /// serilaizes Proxy
        /// </summary>
        /// <returns>
        /// <c>true</c> if the serialization of the Proxy was successfullm, otherwise
        /// <c>false</c>
        /// </returns>
        private Boolean serialize_Proxy()
        {
            try
            {
                // serialize the Proxy
                XMLHandler.Serialize(this.proxy, this.serialization_DirectoryPath + "\\" + this.serialization_filename);

                // successfull
                return true; 
            }
            catch (Exception exception)
            {
                MessageBox.Show("ERROR by XML serialization: \n\n" + exception.Message);

                // Failed
                return false;
            }
        }

        /// <summary>
        /// deserializes Proxy and sets the Parameters of the GUI
        /// </summary>
        private void deserialize_Proxy()
        {
            try
            {
                // if XML File exists
                if (File.Exists(this.serialization_DirectoryPath + "\\" + this.Serialization_Filename)) 
                {
                    // deserialize Proxy
                    this.proxy = (Proxy)XMLHandler.Deserialize<Proxy>(this.serialization_DirectoryPath + "\\" + this.serialization_filename);

                    // set GUI Parameters reading from deserialized proxy
                    this.textBox_WebProxy.Text        = this.proxy.WebProxyAdress;
                    this.textBox_ProxyUsername.Text   = this.proxy.Username;
                    this.textBox_ProxyPassword.Text   = this.proxy.Password;
                    this.checkBox_EnbaleProxy.Checked = this.proxy.Enabled;
                }
            }

            catch (Exception exception)
            {
                MessageBox.Show("ERROR by XML deserialization: \n\n" + exception.Message);
            }
        }

        #endregion
    }
}
