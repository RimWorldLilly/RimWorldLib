using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E1D RID: 7709
	public static class PlayerKnowledgeDatabase
	{
		// Token: 0x0600C84C RID: 51276 RVA: 0x00496D2A File Offset: 0x00494F2A
		static PlayerKnowledgeDatabase()
		{
			PlayerKnowledgeDatabase.ReloadAndRebind();
		}

		// Token: 0x0600C84D RID: 51277 RVA: 0x00496D34 File Offset: 0x00494F34
		public static void ReloadAndRebind()
		{
			PlayerKnowledgeDatabase.data = DirectXmlLoader.ItemFromXmlFile<PlayerKnowledgeDatabase.ConceptKnowledge>(GenFilePaths.ConceptKnowledgeFilePath, true);
			foreach (ConceptDef conceptDef in DefDatabase<ConceptDef>.AllDefs)
			{
				if (!PlayerKnowledgeDatabase.data.knowledge.ContainsKey(conceptDef.defName))
				{
					if (conceptDef != ConceptDefOf.SteamDeckControlsGame && conceptDef != ConceptDefOf.SteamDeckControlsMainMenu)
					{
						string str = "Knowledge data was missing key ";
						ConceptDef conceptDef2 = conceptDef;
						Log.Warning(str + ((conceptDef2 != null) ? conceptDef2.ToString() : null) + ". Adding it...");
					}
					PlayerKnowledgeDatabase.data.knowledge.Add(conceptDef.defName, 0f);
				}
			}
		}

		// Token: 0x0600C84E RID: 51278 RVA: 0x00496DEC File Offset: 0x00494FEC
		public static void ResetPersistent()
		{
			FileInfo fileInfo = new FileInfo(GenFilePaths.ConceptKnowledgeFilePath);
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
			PlayerKnowledgeDatabase.data = new PlayerKnowledgeDatabase.ConceptKnowledge();
		}

		// Token: 0x0600C84F RID: 51279 RVA: 0x00496E1C File Offset: 0x0049501C
		public static void Save()
		{
			try
			{
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(PlayerKnowledgeDatabase.data, typeof(PlayerKnowledgeDatabase.ConceptKnowledge));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.ConceptKnowledgeFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.ConceptKnowledgeFilePath, ex.ToString()));
				string str = "Exception saving knowledge database: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x0600C850 RID: 51280 RVA: 0x00496EB0 File Offset: 0x004950B0
		public static float GetKnowledge(ConceptDef def)
		{
			return PlayerKnowledgeDatabase.data.knowledge[def.defName];
		}

		// Token: 0x0600C851 RID: 51281 RVA: 0x00496EC8 File Offset: 0x004950C8
		public static void SetKnowledge(ConceptDef def, float value)
		{
			float num = PlayerKnowledgeDatabase.data.knowledge[def.defName];
			float num2 = Mathf.Clamp01(value);
			PlayerKnowledgeDatabase.data.knowledge[def.defName] = num2;
			if (num < 0.999f && num2 >= 0.999f)
			{
				PlayerKnowledgeDatabase.NewlyLearned(def);
			}
		}

		// Token: 0x0600C852 RID: 51282 RVA: 0x00496F1C File Offset: 0x0049511C
		public static bool IsComplete(ConceptDef conc)
		{
			return PlayerKnowledgeDatabase.data.knowledge[conc.defName] > 0.999f;
		}

		// Token: 0x0600C853 RID: 51283 RVA: 0x00496F3A File Offset: 0x0049513A
		private static void NewlyLearned(ConceptDef conc)
		{
			TutorSystem.Notify_Event("ConceptLearned-" + conc.defName);
			if (Find.Tutor != null)
			{
				Find.Tutor.learningReadout.Notify_ConceptNewlyLearned(conc);
			}
		}

		// Token: 0x0600C854 RID: 51284 RVA: 0x00496F70 File Offset: 0x00495170
		public static void KnowledgeDemonstrated(ConceptDef conc, KnowledgeAmount know)
		{
			float num;
			switch (know)
			{
			case KnowledgeAmount.FrameDisplayed:
				num = ((Event.current.type == EventType.repaint) ? 0.004f : 0f);
				break;
			case KnowledgeAmount.FrameInteraction:
				num = 0.008f;
				break;
			case KnowledgeAmount.TinyInteraction:
				num = 0.03f;
				break;
			case KnowledgeAmount.SmallInteraction:
				num = 0.1f;
				break;
			case KnowledgeAmount.SpecificInteraction:
				num = 0.4f;
				break;
			case KnowledgeAmount.Total:
				num = 1f;
				break;
			case KnowledgeAmount.NoteClosed:
				num = 0.5f;
				break;
			case KnowledgeAmount.NoteTaught:
				num = 1f;
				break;
			default:
				throw new NotImplementedException();
			}
			if (num <= 0f)
			{
				return;
			}
			PlayerKnowledgeDatabase.SetKnowledge(conc, PlayerKnowledgeDatabase.GetKnowledge(conc) + num);
			LessonAutoActivator.Notify_KnowledgeDemonstrated(conc);
			if (Find.ActiveLesson != null)
			{
				Find.ActiveLesson.Notify_KnowledgeDemonstrated(conc);
			}
		}

		// Token: 0x04007200 RID: 29184
		private static PlayerKnowledgeDatabase.ConceptKnowledge data;

		// Token: 0x02003B5E RID: 15198
		private class ConceptKnowledge
		{
			// Token: 0x0601551E RID: 87326 RVA: 0x006C15E4 File Offset: 0x006BF7E4
			public ConceptKnowledge()
			{
				foreach (ConceptDef conceptDef in DefDatabase<ConceptDef>.AllDefs)
				{
					this.knowledge.Add(conceptDef.defName, 0f);
				}
			}

			// Token: 0x0400EB5E RID: 60254
			public Dictionary<string, float> knowledge = new Dictionary<string, float>();
		}
	}
}
