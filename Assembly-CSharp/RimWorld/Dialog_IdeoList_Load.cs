using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD1 RID: 7377
	public class Dialog_IdeoList_Load : Dialog_IdeoList
	{
		// Token: 0x0600BDBA RID: 48570 RVA: 0x00442A82 File Offset: 0x00440C82
		public Dialog_IdeoList_Load(Action<Ideo> ideoReturner)
		{
			this.interactButLabel = "LoadGameButton".Translate();
			this.ideoReturner = ideoReturner;
		}

		// Token: 0x0600BDBB RID: 48571 RVA: 0x00442AA6 File Offset: 0x00440CA6
		public Dialog_IdeoList_Load(Action<Ideo> ideoReturner, List<Faction> npcFactions, bool devEditMode) : this(ideoReturner)
		{
			this.npcFactions = npcFactions;
			this.devEditMode = devEditMode;
		}

		// Token: 0x0600BDBC RID: 48572 RVA: 0x00442AC0 File Offset: 0x00440CC0
		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths.AbsPathForIdeo(fileName);
			Action <>9__2;
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.Ideo, delegate
			{
				HashSet<string> hashSet = new HashSet<string>();
				Ideo ideo;
				if (!GameDataSaveLoader.TryLoadIdeo(filePath, out ideo))
				{
					this.Close(true);
					return;
				}
				if (!this.devEditMode && !this.npcFactions.NullOrEmpty<Faction>())
				{
					foreach (Faction faction in this.npcFactions)
					{
						foreach (MemeDef memeDef in ideo.memes)
						{
							if (!IdeoUtility.IsMemeAllowedFor(memeDef, faction.def))
							{
								hashSet.Add("UnableToLoadIdeoMemeNotAllowed".Translate(memeDef.label.Named("MEME"), faction.def.label.Named("FACTIONTYPE")));
							}
						}
						if (faction.def.requiredMemes != null)
						{
							foreach (MemeDef memeDef2 in faction.def.requiredMemes)
							{
								if (!ideo.HasMeme(memeDef2))
								{
									hashSet.Add("UnableToLoadIdeoMemeRequired".Translate(memeDef2.label.Named("MEME"), faction.def.label.Named("FACTIONTYPE")));
								}
							}
						}
					}
				}
				if (hashSet.Count == 0)
				{
					this.ideoReturner(IdeoGenerator.InitLoadedIdeo(ideo));
					this.Close(true);
					return;
				}
				TaggedString taggedString = "UnableToLoadIdeoDescription".Translate() + "\n\n" + (from txt in hashSet.AsEnumerable<string>()
				orderby txt
				select txt).ToLineList(" - ", false);
				WindowStack windowStack = Find.WindowStack;
				TaggedString text = taggedString;
				Action confirmedAct;
				if ((confirmedAct = <>9__2) == null)
				{
					confirmedAct = (<>9__2 = delegate()
					{
						this.Close(true);
					});
				}
				windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, "UnableToLoadIdeoTitle".Translate(), WindowLayer.Dialog));
			}, false);
		}

		// Token: 0x04006CAA RID: 27818
		private Action<Ideo> ideoReturner;

		// Token: 0x04006CAB RID: 27819
		public List<Faction> npcFactions;

		// Token: 0x04006CAC RID: 27820
		public bool devEditMode;
	}
}
