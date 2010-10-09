using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using OpenPOP.MIME;
using OpenPOP.POP3;
using OpenPOP.Shared.Logging;
using Attachment = OpenPOP.MIME.Attachment;
using Message = OpenPOP.MIME.Message;

namespace OpenPOP.NET_Sample_App
{
	public class frmTest : Form
	{
		private readonly Hashtable messages = new Hashtable();
		private readonly POPClient popClient;
		private Button ConnectAndRetrieveButton;
		private Button UIDLButton;
		private Panel attachmentPanel;
		private ContextMenu contextMenuMessages;
		private DataGrid gridHeaders;
		private Label label1;
		private Label label2;
		private Label label3;
		private Label label4;
		private Label label5;
		private Label label6;
		private Label label7;
		private Label label8;
		private TreeView listAttachments;
		private TreeView listMessages;
		private ListBox listOfEvents;
		private MenuItem menuDeleteMessage;
		private MenuItem menuViewSource;
		private Panel panel1;
		private Panel panel2;
		private Panel panel3;
		private Panel panel4;
		private Panel panel5;
		private SaveFileDialog saveFile;
		private TextBox loginTextBox;
		private TextBox messageTextBox;
		private TextBox popServerTextBox;
		private TextBox passwordTextBox;
		private TextBox portTextBox;
		private TextBox totalMessagesTextBox;
		private CheckBox useSslCheckBox;

		private frmTest()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// User defined stuff here 
			//

			// This is how you would override the default logger type,
			// typically the application would just pass in the ILog interface object using the constructor
			DefaultLogger.LoggerFactory = AppLoggerFactory;

			popClient = new POPClient( /* new DiagnosticsLogger() */ );
			popClient.AuthenticationBegan     += popClient_AuthenticationBegan;
			popClient.AuthenticationFinished  += popClient_AuthenticationFinished;
			popClient.CommunicationBegan      += popClient_CommunicationBegan;
			popClient.CommunicationOccurred   += popClient_CommunicationOccurred;
			popClient.CommunicationLost       += popClient_CommunicationLost;
			popClient.MessageTransferBegan    += popClient_MessageTransferBegan;
			popClient.MessageTransferFinished += popClient_MessageTransferFinished;

			// This is only for faster debugging purposes
			string myDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string file = Path.Combine(myDocs, "OpenPOPLogin.txt");
			if (File.Exists(file))
			{
				using (StreamReader reader = new StreamReader(File.OpenRead(file)))
				{
					popServerTextBox.Text = reader.ReadLine();
					portTextBox.Text = reader.ReadLine();
					useSslCheckBox.Checked = bool.Parse(reader.ReadLine() ?? "true");
					loginTextBox.Text = reader.ReadLine();
					passwordTextBox.Text = reader.ReadLine();
				}
			}

			FileLogger.Enabled = true;
			FileLogger.Verbose = true;
		}

		#region Windows Form Designer generated code
		/// <summary>
		///   Required method for Designer support - do not modify
		///   the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.useSslCheckBox = new System.Windows.Forms.CheckBox();
			this.UIDLButton = new System.Windows.Forms.Button();
			this.totalMessagesTextBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.passwordTextBox = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.loginTextBox = new System.Windows.Forms.TextBox();
			this.ConnectAndRetrieveButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.portTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.popServerTextBox = new System.Windows.Forms.TextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.gridHeaders = new System.Windows.Forms.DataGrid();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.listOfEvents = new System.Windows.Forms.ListBox();
			this.messageTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.panel5 = new System.Windows.Forms.Panel();
			this.listMessages = new System.Windows.Forms.TreeView();
			this.contextMenuMessages = new System.Windows.Forms.ContextMenu();
			this.menuDeleteMessage = new System.Windows.Forms.MenuItem();
			this.label5 = new System.Windows.Forms.Label();
			this.attachmentPanel = new System.Windows.Forms.Panel();
			this.listAttachments = new System.Windows.Forms.TreeView();
			this.label3 = new System.Windows.Forms.Label();
			this.saveFile = new System.Windows.Forms.SaveFileDialog();
			this.menuViewSource = new System.Windows.Forms.MenuItem();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridHeaders)).BeginInit();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel5.SuspendLayout();
			this.attachmentPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.useSslCheckBox);
			this.panel1.Controls.Add(this.UIDLButton);
			this.panel1.Controls.Add(this.totalMessagesTextBox);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.passwordTextBox);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.loginTextBox);
			this.panel1.Controls.Add(this.ConnectAndRetrieveButton);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.portTextBox);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.popServerTextBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(804, 64);
			this.panel1.TabIndex = 0;
			// 
			// useSslCheckBox
			// 
			this.useSslCheckBox.AutoSize = true;
			this.useSslCheckBox.Checked = true;
			this.useSslCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.useSslCheckBox.Location = new System.Drawing.Point(19, 38);
			this.useSslCheckBox.Name = "useSslCheckBox";
			this.useSslCheckBox.Size = new System.Drawing.Size(68, 17);
			this.useSslCheckBox.TabIndex = 4;
			this.useSslCheckBox.Text = "Use SSL";
			this.useSslCheckBox.UseVisualStyleBackColor = true;
			// 
			// UIDLButton
			// 
			this.UIDLButton.Location = new System.Drawing.Point(460, 42);
			this.UIDLButton.Name = "UIDLButton";
			this.UIDLButton.Size = new System.Drawing.Size(82, 21);
			this.UIDLButton.TabIndex = 6;
			this.UIDLButton.Text = "UIDL";
			this.UIDLButton.Click += new System.EventHandler(this.UIDLButtonClick);
			// 
			// totalMessagesTextBox
			// 
			this.totalMessagesTextBox.Location = new System.Drawing.Point(553, 30);
			this.totalMessagesTextBox.Name = "totalMessagesTextBox";
			this.totalMessagesTextBox.Size = new System.Drawing.Size(100, 20);
			this.totalMessagesTextBox.TabIndex = 7;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(553, 7);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 23);
			this.label6.TabIndex = 9;
			this.label6.Text = "Total Messages";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(264, 36);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(64, 23);
			this.label7.TabIndex = 8;
			this.label7.Text = "Password";
			// 
			// passwordTextBox
			// 
			this.passwordTextBox.Location = new System.Drawing.Point(328, 36);
			this.passwordTextBox.Name = "passwordTextBox";
			this.passwordTextBox.PasswordChar = '*';
			this.passwordTextBox.Size = new System.Drawing.Size(128, 20);
			this.passwordTextBox.TabIndex = 2;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(264, 5);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(40, 23);
			this.label8.TabIndex = 6;
			this.label8.Text = "Login";
			// 
			// loginTextBox
			// 
			this.loginTextBox.Location = new System.Drawing.Point(328, 5);
			this.loginTextBox.Name = "loginTextBox";
			this.loginTextBox.Size = new System.Drawing.Size(128, 20);
			this.loginTextBox.TabIndex = 1;
			// 
			// ConnectAndRetrieveButton
			// 
			this.ConnectAndRetrieveButton.Location = new System.Drawing.Point(460, 0);
			this.ConnectAndRetrieveButton.Name = "ConnectAndRetrieveButton";
			this.ConnectAndRetrieveButton.Size = new System.Drawing.Size(82, 39);
			this.ConnectAndRetrieveButton.TabIndex = 5;
			this.ConnectAndRetrieveButton.Text = "Connect and Retreive";
			this.ConnectAndRetrieveButton.Click += new System.EventHandler(this.ConnectAndRetrieveButtonClick);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(97, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "Port";
			// 
			// portTextBox
			// 
			this.portTextBox.Location = new System.Drawing.Point(128, 39);
			this.portTextBox.Name = "portTextBox";
			this.portTextBox.Size = new System.Drawing.Size(128, 20);
			this.portTextBox.TabIndex = 3;
			this.portTextBox.Text = "110";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "POP Server Address";
			// 
			// popServerTextBox
			// 
			this.popServerTextBox.Location = new System.Drawing.Point(128, 8);
			this.popServerTextBox.Name = "popServerTextBox";
			this.popServerTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.popServerTextBox.Size = new System.Drawing.Size(128, 20);
			this.popServerTextBox.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.gridHeaders);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 260);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(804, 184);
			this.panel2.TabIndex = 1;
			// 
			// gridHeaders
			// 
			this.gridHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                                 | System.Windows.Forms.AnchorStyles.Left)
			                                                                | System.Windows.Forms.AnchorStyles.Right)));
			this.gridHeaders.DataMember = "";
			this.gridHeaders.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.gridHeaders.Location = new System.Drawing.Point(0, 0);
			this.gridHeaders.Name = "gridHeaders";
			this.gridHeaders.PreferredColumnWidth = 400;
			this.gridHeaders.ReadOnly = true;
			this.gridHeaders.Size = new System.Drawing.Size(804, 188);
			this.gridHeaders.TabIndex = 3;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.panel4);
			this.panel3.Controls.Add(this.panel5);
			this.panel3.Controls.Add(this.attachmentPanel);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 64);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(804, 196);
			this.panel3.TabIndex = 2;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.listOfEvents);
			this.panel4.Controls.Add(this.messageTextBox);
			this.panel4.Controls.Add(this.label4);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(175, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(492, 196);
			this.panel4.TabIndex = 6;
			// 
			// listOfEvents
			// 
			this.listOfEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
			                                                              | System.Windows.Forms.AnchorStyles.Right)));
			this.listOfEvents.Location = new System.Drawing.Point(7, 171);
			this.listOfEvents.Name = "lstEvents";
			this.listOfEvents.Size = new System.Drawing.Size(475, 17);
			this.listOfEvents.TabIndex = 8;
			// 
			// messageTextBox
			// 
			this.messageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                                | System.Windows.Forms.AnchorStyles.Left)
			                                                               | System.Windows.Forms.AnchorStyles.Right)));
			this.messageTextBox.Location = new System.Drawing.Point(7, 22);
			this.messageTextBox.MaxLength = 999999999;
			this.messageTextBox.Multiline = true;
			this.messageTextBox.Name = "messageTextBox";
			this.messageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.messageTextBox.Size = new System.Drawing.Size(476, 143);
			this.messageTextBox.TabIndex = 9;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(136, 16);
			this.label4.TabIndex = 5;
			this.label4.Text = "Message Body";
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.listMessages);
			this.panel5.Controls.Add(this.label5);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel5.Location = new System.Drawing.Point(0, 0);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(175, 196);
			this.panel5.TabIndex = 5;
			// 
			// listMessages
			// 
			this.listMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                                  | System.Windows.Forms.AnchorStyles.Left)
			                                                                 | System.Windows.Forms.AnchorStyles.Right)));
			this.listMessages.ContextMenu = this.contextMenuMessages;
			this.listMessages.Location = new System.Drawing.Point(8, 24);
			this.listMessages.Name = "listMessages";
			this.listMessages.ShowLines = false;
			this.listMessages.ShowRootLines = false;
			this.listMessages.Size = new System.Drawing.Size(160, 160);
			this.listMessages.TabIndex = 8;
			this.listMessages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.listMessages_AfterSelect);
			// 
			// contextMenuMessages
			// 
			this.contextMenuMessages.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
			                                    	{
			                                    		this.menuDeleteMessage,
			                                    		this.menuViewSource
			                                    	});
			// 
			// menuDeleteMessage
			// 
			this.menuDeleteMessage.Index = 0;
			this.menuDeleteMessage.Text = "Delete Mail";
			this.menuDeleteMessage.Click += new System.EventHandler(this.mnuDeleteMessage_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(136, 16);
			this.label5.TabIndex = 1;
			this.label5.Text = "Message Number";
			// 
			// attachmentPanel
			// 
			this.attachmentPanel.Controls.Add(this.listAttachments);
			this.attachmentPanel.Controls.Add(this.label3);
			this.attachmentPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.attachmentPanel.Location = new System.Drawing.Point(667, 0);
			this.attachmentPanel.Name = "attachmentPanel";
			this.attachmentPanel.Size = new System.Drawing.Size(137, 196);
			this.attachmentPanel.TabIndex = 4;
			this.attachmentPanel.Visible = false;
			// 
			// listAttachments
			// 
			this.listAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                                     | System.Windows.Forms.AnchorStyles.Left)
			                                                                    | System.Windows.Forms.AnchorStyles.Right)));
			this.listAttachments.Location = new System.Drawing.Point(8, 24);
			this.listAttachments.Name = "listAttachments";
			this.listAttachments.ShowLines = false;
			this.listAttachments.ShowRootLines = false;
			this.listAttachments.Size = new System.Drawing.Size(121, 160);
			this.listAttachments.TabIndex = 10;
			this.listAttachments.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.listAttachments_AfterSelect);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(136, 16);
			this.label3.TabIndex = 3;
			this.label3.Text = "Attachments";
			// 
			// saveFile
			// 
			this.saveFile.Title = "Save Attachment";
			// 
			// menuViewSource
			// 
			this.menuViewSource.Index = 1;
			this.menuViewSource.Text = "View source";
			this.menuViewSource.Click += new System.EventHandler(this.mnuDeleteMessageClick);
			// 
			// frmTest
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(804, 444);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "frmTest";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "OpenPOP.NET Sample Application";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridHeaders)).EndInit();
			this.panel3.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panel5.ResumeLayout(false);
			this.attachmentPanel.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion

		private static ILog AppLoggerFactory()
		{
			return new FileLogger();
		}

		/// <summary>
		///   The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Application.Run(new frmTest());
		}

		private void ReceiveMails()
		{
			// Disable buttons while working
			ConnectAndRetrieveButton.Enabled = false;
			UIDLButton.Enabled = false;

			try
			{
				if (popClient.Connected)
					popClient.Disconnect();
				popClient.Connect(popServerTextBox.Text, int.Parse(portTextBox.Text), useSslCheckBox.Checked);
				popClient.Authenticate(loginTextBox.Text, passwordTextBox.Text);
				int Count = popClient.GetMessageCount();
				totalMessagesTextBox.Text = Count.ToString();
				messageTextBox.Text = "";
				messages.Clear();
				listMessages.Nodes.Clear();
				listAttachments.Nodes.Clear();

				int success = 0;
				int fail = 0;
				for (int i = Count; i >= 1; i -= 1)
				{
					// Check if the form is closed while we are working. If so, abort
					if (IsDisposed)
						return;

					// Refresh the form while fetching emails
					// This will fix the "Application is not responding" problem
					Application.DoEvents();

					try
					{
						Message m = popClient.GetMessage(i);

						success++;
						messages.Add("msg" + i, m);
						TreeNode node = listMessages.Nodes.Add("[" + i + "] " + m.Headers.Subject);
						node.Tag = i.ToString();
					} catch (Exception)
					{
						fail++;
					}
				}
				MessageBox.Show(this, "Mail received!\nSuccess: " + success + "\nFailed: " + fail);
			} catch (InvalidLoginException)
			{
				MessageBox.Show(this, "Unknown username!", "POP3 Server Authentication");
			} catch (InvalidPasswordException)
			{
				MessageBox.Show(this, "Invalid password!", "POP3 Server Authentication");
			} catch (PopServerNotFoundException)
			{
				MessageBox.Show(this, "The server could not be found", "POP3 Retrieval");
			} catch (Exception e)
			{
				MessageBox.Show(this, "Error occurred retrieving mail. " + e.Message, "POP3 Retrieval");
			} finally
			{
				// Enable the buttons again
				ConnectAndRetrieveButton.Enabled = true;
				UIDLButton.Enabled = true;
			}
		}

		private void ConnectAndRetrieveButtonClick(object sender, EventArgs e)
		{
			ReceiveMails();
		}

		private void listMessages_AfterSelect(object sender, TreeViewEventArgs e)
		{
			Message m = (Message)messages["msg" + listMessages.SelectedNode.Tag];
			if (m != null)
			{
				if (m.MessageBody.Count > 0)
				{
					// Find the first text/plain version
					bool messageSet = false;
					foreach (MessageBody messageBody in m.MessageBody)
					{
						if (messageBody.Type.ToLower().Equals("text/plain"))
						{
							messageTextBox.Text = messageBody.Body;
							messageSet = true;
							break;
						}
					}

					if (!messageSet)
						messageTextBox.Text = m.MessageBody[0].Body;
				}
				listAttachments.Nodes.Clear();

				bool hadAttachments = false;
				foreach (Attachment att in m.Attachments)
				{
					hadAttachments = true;
					listAttachments.Nodes.Add(att.ContentFileName).Tag = att;
				}

				attachmentPanel.Visible = hadAttachments;

				DataSet ds = new DataSet();
				ds.Tables.Add("table1");
				ds.Tables[0].Columns.Add("Header");
				ds.Tables[0].Columns.Add("Value");

				ds.Tables[0].Rows.Add(new object[] {"ContentType", m.Headers.ContentType});
				ds.Tables[0].Rows.Add(new object[] {"AttachmentCount", m.Attachments.Count});

				foreach (MailAddress CC in m.Headers.CC)
					ds.Tables[0].Rows.Add(new object[] {"CC", CC});
				foreach (MailAddress To in m.Headers.To)
					ds.Tables[0].Rows.Add(new object[] {"To", To});

				ds.Tables[0].Rows.Add(new object[] {"ContentTransferEncoding", m.Headers.ContentTransferEncoding});
				ds.Tables[0].Rows.Add(new object[] {"From", m.Headers.From});
				ds.Tables[0].Rows.Add(new object[] {"MessageID", m.Headers.MessageID});
				ds.Tables[0].Rows.Add(new object[] {"MimeVersion", m.Headers.MimeVersion});
				ds.Tables[0].Rows.Add(new object[] {"ReturnPath", m.Headers.ReturnPath});
				ds.Tables[0].Rows.Add(new object[] {"Subject", m.Headers.Subject});
				ds.Tables[0].Rows.Add(new object[] {"Date", m.Headers.Date});
				ds.Tables[0].Rows.Add(new object[] {"DateSent", m.Headers.DateSent});
				foreach (string received in m.Headers.Received)
					ds.Tables[0].Rows.Add(new object[] {"Received", received});
				ds.Tables[0].Rows.Add(new object[] {"Importance", m.Headers.Importance});
				ds.Tables[0].Rows.Add(new object[] {"ReplyTo", m.Headers.ReplyTo});
				foreach (string keyword in m.Headers.Keywords)
					ds.Tables[0].Rows.Add(new object[] {"Keyword", keyword});
				foreach (string key in m.Headers.UnknownHeaders)
				{
					string[] values = m.Headers.UnknownHeaders.GetValues(key);
					if (values != null)
						foreach (string value in values)
						{
							ds.Tables[0].Rows.Add(new object[] {key, value});
						}
				}
				gridHeaders.DataMember = ds.Tables[0].TableName;
				gridHeaders.DataSource = ds;
			}
		}

		private void listAttachments_AfterSelect(object sender, TreeViewEventArgs e)
		{
			Attachment att = (Attachment)listAttachments.SelectedNode.Tag;
			Message m = (Message)messages["msg" + listMessages.SelectedNode.Tag];
			if (att != null && m != null)
			{
				saveFile.FileName = att.ContentFileName;
				DialogResult result = saveFile.ShowDialog();
				if (result != DialogResult.OK)
					return;

				if (att.IsMIMEMailFile())
				{
					result = MessageBox.Show(this, "OpenPOP.POP3 found the attachment is a MIME mail, do you want to extract it?", "MIME mail", MessageBoxButtons.YesNo);
					if (result == DialogResult.Yes)
					{
						Message m2 = att.DecodeAsMessage(true, false);
						string attachmentNames = "";
						if (m2.Attachments.Count > 0)
						{
							foreach (Attachment att2 in m2.Attachments)
							{
								attachmentNames += att2.ContentFileName + "(" + att2.RawAttachment.Length + " bytes)\r\n";
							}
						}

						bool saveSuccesfull = false;
						string directoryPath = Path.GetDirectoryName(saveFile.FileName);
						if (directoryPath != null)
							saveSuccesfull = m.SaveAttachments(new DirectoryInfo(directoryPath));
						MessageBox.Show(this, "Parsing " + (saveSuccesfull ? "succeeded" : "failed") + "\r\n\r\nsubject:" + m2.Headers.Subject + "\r\n\r\nAttachment:\r\n" + attachmentNames);
					}
				}
				MessageBox.Show(this, "Attachment saving " + ((att.SaveToFile(new FileInfo(saveFile.FileName))) ? "succeeded" : "failed"));
			} else
				MessageBox.Show(this, "attachment object is null!");
		}

		private void mnuDeleteMessage_Click(object sender, EventArgs e)
		{
			if (listMessages.SelectedNode != null)
			{
				DialogResult drRet = MessageBox.Show(this, "Are you sure to delete the email?", "Delete email", MessageBoxButtons.YesNo);
				if (drRet == DialogResult.Yes)
				{
					popClient.DeleteMessage(Convert.ToInt32(listMessages.SelectedNode.Tag));

					listMessages.SelectedNode.Remove();

					drRet = MessageBox.Show(this, "Do you want to receive email again?", "Receive email", MessageBoxButtons.YesNo);
					if (drRet == DialogResult.Yes)
						ReceiveMails();
				}
			}
		}

		private void UIDLButtonClick(object sender, EventArgs e)
		{
			List<string> uids = popClient.GetMessageUIDs();

			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UIDL:\r\n");
			foreach (string uid in uids)
			{
				stringBuilder.Append(uid + "\r\n");
			}

			messageTextBox.Text = stringBuilder.ToString();
		}

		private void AddEvent(string strEvent)
		{
			listOfEvents.Items.Add(strEvent);
			listOfEvents.SelectedIndex = listOfEvents.Items.Count - 1;
		}

		private void popClient_CommunicationBegan(POPClient sender)
		{
			AddEvent("CommunicationBegan");
		}

		private void popClient_CommunicationOccurred(POPClient sender)
		{
			AddEvent("CommunicationOccurred");
		}

		private void popClient_AuthenticationBegan(POPClient sender)
		{
			AddEvent("AuthenticationBegan");
		}

		private void popClient_AuthenticationFinished(POPClient sender)
		{
			AddEvent("AuthenticationFinished");
		}

		private void popClient_MessageTransferBegan(POPClient sender)
		{
			AddEvent("MessageTransferBegan");
		}

		private void popClient_MessageTransferFinished(POPClient sender)
		{
			AddEvent("MessageTransferFinished");
		}

		private void popClient_CommunicationLost(POPClient sender)
		{
			AddEvent("CommunicationLost");
		}

		private void mnuDeleteMessageClick(object sender, EventArgs e)
		{
			if (listMessages.SelectedNode != null)
			{
				Message m = (Message)messages["msg" + listMessages.SelectedNode.Tag];

				ShowSourceForm sourceForm = new ShowSourceForm(m.RawMessage);
				sourceForm.ShowDialog();
			}
		}
	}
}