using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000525 RID: 1317
	public class ScribeLoader
	{
		// Token: 0x060026F8 RID: 9976 RVA: 0x000F266C File Offset: 0x000F086C
		public void InitLoading(string filePath)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitLoading() but current mode is " + Scribe.mode.ToString());
				Scribe.ForceStop();
			}
			if (this.curParent != null)
			{
				Log.Error("Current parent is not null in InitLoading");
				this.curParent = null;
			}
			if (this.curPathRelToParent != null)
			{
				Log.Error("Current path relative to parent is not null in InitLoading");
				this.curPathRelToParent = null;
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(xmlTextReader);
						this.curXmlParent = xmlDocument.DocumentElement;
					}
				}
				Scribe.mode = LoadSaveMode.LoadingVars;
			}
			catch (Exception ex)
			{
				string str = "Exception while init loading file: ";
				string str2 = "\n";
				Exception ex2 = ex;
				Log.Error(str + filePath + str2 + ((ex2 != null) ? ex2.ToString() : null));
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x000F2774 File Offset: 0x000F0974
		public void InitLoadingMetaHeaderOnly(string filePath)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitLoadingMetaHeaderOnly() but current mode is " + Scribe.mode.ToString());
				Scribe.ForceStop();
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						if (ScribeMetaHeaderUtility.ReadToMetaElement(xmlTextReader))
						{
							using (XmlReader xmlReader = xmlTextReader.ReadSubtree())
							{
								XmlDocument xmlDocument = new XmlDocument();
								xmlDocument.Load(xmlReader);
								XmlElement xmlElement = xmlDocument.CreateElement("root");
								xmlElement.AppendChild(xmlDocument.DocumentElement);
								this.curXmlParent = xmlElement;
								goto IL_87;
							}
							goto IL_85;
							IL_87:
							goto IL_93;
						}
						IL_85:
						return;
					}
					IL_93:;
				}
				Scribe.mode = LoadSaveMode.LoadingVars;
			}
			catch (Exception ex)
			{
				string str = "Exception while init loading meta header: ";
				string str2 = "\n";
				Exception ex2 = ex;
				Log.Error(str + filePath + str2 + ((ex2 != null) ? ex2.ToString() : null));
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x000F288C File Offset: 0x000F0A8C
		public void FinalizeLoading()
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Called FinalizeLoading() but current mode is " + Scribe.mode.ToString());
				return;
			}
			try
			{
				Scribe.ExitNode();
				this.curXmlParent = null;
				this.curParent = null;
				this.curPathRelToParent = null;
				Scribe.mode = LoadSaveMode.Inactive;
				DeepProfiler.Start("ResolveAllCrossReferences()");
				this.crossRefs.ResolveAllCrossReferences();
				DeepProfiler.End();
				DeepProfiler.Start("DoAllPostLoadInits()");
				this.initer.DoAllPostLoadInits();
				DeepProfiler.End();
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

		// Token: 0x060026FB RID: 9979 RVA: 0x000F2950 File Offset: 0x000F0B50
		public bool EnterNode(string nodeName)
		{
			if (this.curXmlParent != null)
			{
				XmlNode xmlNode = this.curXmlParent[nodeName];
				if (xmlNode == null && char.IsDigit(nodeName[0]))
				{
					xmlNode = this.curXmlParent.ChildNodes[int.Parse(nodeName)];
				}
				if (xmlNode == null)
				{
					return false;
				}
				this.curXmlParent = xmlNode;
			}
			this.curPathRelToParent = this.curPathRelToParent + "/" + nodeName;
			return true;
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x000F29C0 File Offset: 0x000F0BC0
		public void ExitNode()
		{
			if (this.curXmlParent != null)
			{
				this.curXmlParent = this.curXmlParent.ParentNode;
			}
			if (this.curPathRelToParent != null)
			{
				int num = this.curPathRelToParent.LastIndexOf('/');
				this.curPathRelToParent = ((num > 0) ? this.curPathRelToParent.Substring(0, num) : null);
			}
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x000F2A18 File Offset: 0x000F0C18
		public void ForceStop()
		{
			this.curXmlParent = null;
			this.curParent = null;
			this.curPathRelToParent = null;
			this.crossRefs.Clear(false);
			this.initer.Clear();
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}

		// Token: 0x04001C08 RID: 7176
		public CrossRefHandler crossRefs = new CrossRefHandler();

		// Token: 0x04001C09 RID: 7177
		public PostLoadIniter initer = new PostLoadIniter();

		// Token: 0x04001C0A RID: 7178
		public IExposable curParent;

		// Token: 0x04001C0B RID: 7179
		public XmlNode curXmlParent;

		// Token: 0x04001C0C RID: 7180
		public string curPathRelToParent;
	}
}
