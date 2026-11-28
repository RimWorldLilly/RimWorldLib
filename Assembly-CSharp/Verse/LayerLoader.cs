using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml.XPath;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000663 RID: 1635
	public static class LayerLoader
	{
		// Token: 0x060033E1 RID: 13281 RVA: 0x0014DCFC File Offset: 0x0014BEFC
		public static void LoadFileIntoList(TextAsset ass, List<DiaNodeMold> NodeListToFill, List<DiaNodeList> ListListToFill, DiaNodeType NodesType)
		{
			XPathNavigator xpathNavigator = new XPathDocument(new StringReader(ass.text)).CreateNavigator();
			xpathNavigator.MoveToFirst();
			xpathNavigator.MoveToFirstChild();
			foreach (object obj in xpathNavigator.Select("Node"))
			{
				XPathNavigator xpathNavigator2 = (XPathNavigator)obj;
				try
				{
					TextReader textReader = new StringReader(xpathNavigator2.OuterXml);
					DiaNodeMold diaNodeMold = (DiaNodeMold)new XmlSerializer(typeof(DiaNodeMold)).Deserialize(textReader);
					diaNodeMold.nodeType = NodesType;
					NodeListToFill.Add(diaNodeMold);
					textReader.Dispose();
				}
				catch (Exception ex)
				{
					string str = "Exception deserializing ";
					string outerXml = xpathNavigator2.OuterXml;
					string str2 = ":\n";
					Exception innerException = ex.InnerException;
					Log.Message(str + outerXml + str2 + ((innerException != null) ? innerException.ToString() : null));
				}
			}
			foreach (object obj2 in xpathNavigator.Select("NodeList"))
			{
				XPathNavigator xpathNavigator3 = (XPathNavigator)obj2;
				try
				{
					TextReader textReader2 = new StringReader(xpathNavigator3.OuterXml);
					DiaNodeList item = (DiaNodeList)new XmlSerializer(typeof(DiaNodeList)).Deserialize(textReader2);
					ListListToFill.Add(item);
				}
				catch (Exception ex2)
				{
					string str3 = "Exception deserializing ";
					string outerXml2 = xpathNavigator3.OuterXml;
					string str4 = ":\n";
					Exception innerException2 = ex2.InnerException;
					Log.Message(str3 + outerXml2 + str4 + ((innerException2 != null) ? innerException2.ToString() : null));
				}
			}
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x0014DEB8 File Offset: 0x0014C0B8
		public static void MarkNonRootNodes(List<DiaNodeMold> NodeList)
		{
			foreach (DiaNodeMold d in NodeList)
			{
				LayerLoader.RecursiveSetIsRootFalse(d);
			}
			foreach (DiaNodeMold diaNodeMold in NodeList)
			{
				foreach (DiaNodeMold diaNodeMold2 in NodeList)
				{
					foreach (DiaOptionMold diaOptionMold in diaNodeMold2.optionList)
					{
						bool flag = false;
						using (List<string>.Enumerator enumerator4 = diaOptionMold.ChildNodeNames.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								if (enumerator4.Current == diaNodeMold.name)
								{
									flag = true;
								}
							}
						}
						if (flag)
						{
							diaNodeMold.isRoot = false;
						}
					}
				}
			}
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x0014E00C File Offset: 0x0014C20C
		private static void RecursiveSetIsRootFalse(DiaNodeMold d)
		{
			foreach (DiaOptionMold diaOptionMold in d.optionList)
			{
				foreach (DiaNodeMold diaNodeMold in diaOptionMold.ChildNodes)
				{
					diaNodeMold.isRoot = false;
					LayerLoader.RecursiveSetIsRootFalse(diaNodeMold);
				}
			}
		}
	}
}
