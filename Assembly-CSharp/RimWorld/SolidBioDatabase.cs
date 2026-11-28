using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200154C RID: 5452
	public class SolidBioDatabase
	{
		// Token: 0x06008538 RID: 34104 RVA: 0x003005ED File Offset: 0x002FE7ED
		public static void Clear()
		{
			SolidBioDatabase.allBios.Clear();
			PawnNameDatabaseSolid.Clear();
		}

		// Token: 0x06008539 RID: 34105 RVA: 0x00300600 File Offset: 0x002FE800
		public static void LoadAllBios()
		{
			TextAsset[] array = Resources.LoadAll<TextAsset>("Backstories/Solid");
			List<PawnBio> list = new List<PawnBio>();
			foreach (TextAsset textAsset in array)
			{
				try
				{
					SolidBioDatabase.ParseTextAsset(textAsset, list);
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to parse text asset ");
					defaultInterpolatedStringHandler.AppendFormatted(textAsset.name);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			foreach (PawnBio pawnBio in list)
			{
				pawnBio.name.ResolveMissingPieces(null);
				if (pawnBio.childhood == null || pawnBio.adulthood == null)
				{
					PawnNameDatabaseSolid.AddPlayerContentName(pawnBio.name, pawnBio.gender);
				}
				else
				{
					pawnBio.ResolveReferences();
					SolidBioDatabase.allBios.Add(pawnBio);
				}
			}
		}

		// Token: 0x0600853A RID: 34106 RVA: 0x00300710 File Offset: 0x002FE910
		private static void ParseTextAsset(TextAsset textObject, List<PawnBio> outputBios)
		{
			StringReader stringReader = new StringReader(textObject.text);
			XmlReader xmlReader = XmlReader.Create(stringReader, SolidBioDatabase.Settings);
			Dictionary<string, PawnBio> dictionary = new Dictionary<string, PawnBio>();
			PawnBio pawnBio = null;
			bool flag = false;
			string text = null;
			bool flag2 = false;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			while (xmlReader.Read())
			{
				XmlNodeType nodeType = xmlReader.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType == XmlNodeType.EndElement)
					{
						if (xmlReader.Name == "PawnBio")
						{
							if (text2 != null || text3 != null || text4 != null)
							{
								pawnBio.name = new NameTriple(text2, text4, text3);
							}
							if (flag && text != null)
							{
								dictionary[text] = pawnBio;
							}
							else
							{
								outputBios.Add(pawnBio);
							}
							pawnBio = null;
							flag = false;
							text = null;
						}
						else if (xmlReader.Name == "Name")
						{
							flag2 = false;
						}
					}
				}
				else if (xmlReader.Name == "PawnBio")
				{
					flag = (xmlReader.GetAttribute("Abstract") == "True");
					string attribute = xmlReader.GetAttribute("ParentName");
					text = xmlReader.GetAttribute("Name");
					text2 = null;
					text3 = null;
					text4 = null;
					PawnBio pawnBio2;
					if (attribute != null && dictionary.TryGetValue(attribute, out pawnBio2))
					{
						pawnBio = new PawnBio
						{
							gender = pawnBio2.gender,
							childhood = pawnBio2.childhood,
							adulthood = pawnBio2.adulthood,
							pirateKing = pawnBio2.pirateKing,
							rare = pawnBio2.rare
						};
					}
					else
					{
						pawnBio = new PawnBio();
					}
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Name"))
				{
					flag2 = true;
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "First") || (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "firstInt") && flag2))
				{
					text2 = xmlReader.ReadString();
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Last") || (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "lastInt") && flag2))
				{
					text3 = xmlReader.ReadString();
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Nick") || (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "nickInt") && flag2))
				{
					text4 = xmlReader.ReadString();
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Childhood"))
				{
					string defName = xmlReader.ReadString();
					pawnBio.childhood = DefDatabase<BackstoryDef>.GetNamed(defName, false);
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Adulthood"))
				{
					string defName2 = xmlReader.ReadString();
					pawnBio.adulthood = DefDatabase<BackstoryDef>.GetNamed(defName2, false);
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Gender"))
				{
					GenderPossibility gender;
					if (Enum.TryParse<GenderPossibility>(xmlReader.ReadString(), out gender))
					{
						pawnBio.gender = gender;
					}
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "PirateKing"))
				{
					pawnBio.pirateKing = bool.Parse(xmlReader.ReadString());
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Rare"))
				{
					pawnBio.rare = bool.Parse(xmlReader.ReadString());
				}
			}
			xmlReader.Close();
			stringReader.Close();
		}

		// Token: 0x0600853D RID: 34109 RVA: 0x00300A72 File Offset: 0x002FEC72
		[CompilerGenerated]
		internal static bool <ParseTextAsset>g__strcmp|4_0(string a, string b)
		{
			return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x04005202 RID: 20994
		public static List<PawnBio> allBios = new List<PawnBio>();

		// Token: 0x04005203 RID: 20995
		private static readonly XmlReaderSettings Settings = new XmlReaderSettings
		{
			IgnoreComments = true,
			IgnoreWhitespace = true,
			CheckCharacters = false,
			Async = false
		};
	}
}
