using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000529 RID: 1321
	public class ScribeSaver
	{
		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06002704 RID: 9988 RVA: 0x000F2D59 File Offset: 0x000F0F59
		public string CurPath
		{
			get
			{
				return this.curPath;
			}
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000F2D64 File Offset: 0x000F0F64
		public void InitSaving(string filePath, string documentElementName)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitSaving() but current mode is " + Scribe.mode.ToString());
				Scribe.ForceStop();
			}
			if (this.curPath != null)
			{
				Log.Error("Current path is not null in InitSaving");
				this.curPath = null;
				this.savedNodes.Clear();
				this.nextListElementTemporaryId = 0;
			}
			try
			{
				Scribe.mode = LoadSaveMode.Saving;
				this.saveStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Indent = true;
				xmlWriterSettings.IndentChars = "\t";
				this.writer = XmlWriter.Create(this.saveStream, xmlWriterSettings);
				this.writer.WriteStartDocument();
				this.EnterNode(documentElementName);
			}
			catch (Exception ex)
			{
				string str = "Exception while init saving file: ";
				string str2 = "\n";
				Exception ex2 = ex;
				Log.Error(str + filePath + str2 + ((ex2 != null) ? ex2.ToString() : null));
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000F2E5C File Offset: 0x000F105C
		public void FinalizeSaving()
		{
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error("Called FinalizeSaving() but current mode is " + Scribe.mode.ToString());
				return;
			}
			if (this.anyInternalException)
			{
				this.ForceStop();
				throw new Exception("Can't finalize saving due to internal exception. The whole file would be most likely corrupted anyway.");
			}
			try
			{
				if (this.writer != null)
				{
					this.ExitNode();
					this.writer.WriteEndDocument();
					this.writer.Flush();
					this.writer.Close();
					this.writer = null;
				}
				if (this.saveStream != null)
				{
					this.saveStream.Flush();
					this.saveStream.Close();
					this.saveStream = null;
				}
				Scribe.mode = LoadSaveMode.Inactive;
				this.savingForDebug = false;
				this.loadIDsErrorsChecker.CheckForErrorsAndClear();
				this.curPath = null;
				this.savedNodes.Clear();
				this.nextListElementTemporaryId = 0;
				this.anyInternalException = false;
			}
			catch (Exception ex)
			{
				string str = "Exception in FinalizeLoading(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000F2F78 File Offset: 0x000F1178
		public void WriteElement(string elementName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteElemenet(), but writer is null.");
				return;
			}
			try
			{
				this.writer.WriteElementString(elementName, value);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000F2FC4 File Offset: 0x000F11C4
		public void WriteAttribute(string attributeName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteAttribute(), but writer is null.");
				return;
			}
			try
			{
				this.writer.WriteAttributeString(attributeName, value);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000F3010 File Offset: 0x000F1210
		public string DebugOutputFor(IExposable saveable)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("DebugOutput needs current mode to be Inactive");
				return "";
			}
			string result;
			try
			{
				using (StringWriter stringWriter = new StringWriter())
				{
					XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
					xmlWriterSettings.Indent = true;
					xmlWriterSettings.IndentChars = "  ";
					xmlWriterSettings.OmitXmlDeclaration = true;
					try
					{
						using (this.writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
						{
							Scribe.mode = LoadSaveMode.Saving;
							this.savingForDebug = true;
							Scribe_Deep.Look<IExposable>(ref saveable, "saveable", Array.Empty<object>());
						}
						result = stringWriter.ToString();
					}
					finally
					{
						this.ForceStop();
					}
				}
			}
			catch (Exception ex)
			{
				string str = "Exception while getting debug output: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				this.ForceStop();
				result = "";
			}
			return result;
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000F3114 File Offset: 0x000F1314
		public bool EnterNode(string nodeName)
		{
			if (this.writer == null)
			{
				return false;
			}
			try
			{
				this.writer.WriteStartElement(nodeName);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
			return true;
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x000F3158 File Offset: 0x000F1358
		public void ExitNode()
		{
			if (this.writer == null)
			{
				return;
			}
			try
			{
				this.writer.WriteEndElement();
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000F3198 File Offset: 0x000F1398
		public void ForceStop()
		{
			if (this.writer != null)
			{
				this.writer.Close();
				this.writer = null;
			}
			if (this.saveStream != null)
			{
				this.saveStream.Close();
				this.saveStream = null;
			}
			this.savingForDebug = false;
			this.loadIDsErrorsChecker.Clear();
			this.curPath = null;
			this.savedNodes.Clear();
			this.nextListElementTemporaryId = 0;
			this.anyInternalException = false;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}

		// Token: 0x04001C20 RID: 7200
		public DebugLoadIDsSavingErrorsChecker loadIDsErrorsChecker = new DebugLoadIDsSavingErrorsChecker();

		// Token: 0x04001C21 RID: 7201
		public bool savingForDebug;

		// Token: 0x04001C22 RID: 7202
		private Stream saveStream;

		// Token: 0x04001C23 RID: 7203
		private XmlWriter writer;

		// Token: 0x04001C24 RID: 7204
		private string curPath;

		// Token: 0x04001C25 RID: 7205
		private HashSet<string> savedNodes = new HashSet<string>();

		// Token: 0x04001C26 RID: 7206
		private int nextListElementTemporaryId;

		// Token: 0x04001C27 RID: 7207
		private bool anyInternalException;
	}
}
