using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001D06 RID: 7430
	public class Page_ScenarioEditor : Page
	{
		// Token: 0x170022E3 RID: 8931
		// (get) Token: 0x0600BFFC RID: 49148 RVA: 0x0045EF3C File Offset: 0x0045D13C
		public bool ScenarioEdited
		{
			get
			{
				return this.editMode && this.curScen.GetHashCode() != this.initialHash;
			}
		}

		// Token: 0x170022E4 RID: 8932
		// (get) Token: 0x0600BFFD RID: 49149 RVA: 0x0045EF5E File Offset: 0x0045D15E
		public override string PageTitle
		{
			get
			{
				return "ScenarioEditor".Translate() + (this.ScenarioEdited ? "*" : "");
			}
		}

		// Token: 0x170022E5 RID: 8933
		// (get) Token: 0x0600BFFE RID: 49150 RVA: 0x0045EF88 File Offset: 0x0045D188
		public Scenario EditingScenario
		{
			get
			{
				return this.curScen;
			}
		}

		// Token: 0x0600BFFF RID: 49151 RVA: 0x0045EF90 File Offset: 0x0045D190
		public Page_ScenarioEditor(Scenario scen)
		{
			if (scen != null)
			{
				this.curScen = scen;
				this.seedIsValid = false;
			}
			else
			{
				this.RandomizeSeedAndScenario();
			}
			this.initialHash = this.curScen.GetHashCode();
		}

		// Token: 0x0600C000 RID: 49152 RVA: 0x0045EFE6 File Offset: 0x0045D1E6
		public override void PreOpen()
		{
			base.PreOpen();
			this.infoScrollPosition = Vector2.zero;
			this.needsSave = true;
		}

		// Token: 0x0600C001 RID: 49153 RVA: 0x0045F000 File Offset: 0x0045D200
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			Widgets.BeginGroup(mainRect);
			Rect rect2 = new Rect(0f, 0f, mainRect.width * 0.35f, mainRect.height).Rounded();
			this.DoConfigControls(rect2);
			Rect rect3 = new Rect(rect2.xMax + 17f, 0f, mainRect.width - rect2.width - 17f, mainRect.height).Rounded();
			if (!this.editMode)
			{
				ScenarioUI.DrawScenarioInfo(rect3, this.curScen, ref this.infoScrollPosition);
			}
			else
			{
				ScenarioUI.DrawScenarioEditInterface(rect3, this.curScen, ref this.infoScrollPosition);
			}
			Widgets.EndGroup();
			base.DoBottomButtons(rect, null, null, null, true, true);
		}

		// Token: 0x0600C002 RID: 49154 RVA: 0x0045F0D0 File Offset: 0x0045D2D0
		private void RandomizeSeedAndScenario()
		{
			this.seed = GenText.RandomSeedString();
			this.curScen = ScenarioMaker.GenerateNewRandomScenario(this.seed);
		}

		// Token: 0x0600C003 RID: 49155 RVA: 0x0045F0F0 File Offset: 0x0045D2F0
		private void DoConfigControls(Rect rect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = 200f;
			listing_Standard.Begin(rect);
			if (listing_Standard.ButtonText("Load".Translate(), null, 1f))
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Load(delegate(Scenario loadedScen)
				{
					this.curScen = loadedScen;
					this.seedIsValid = false;
				}));
			}
			if (listing_Standard.ButtonText("Save".Translate(), null, 1f) && Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Save(this.curScen, null));
			}
			if (listing_Standard.ButtonText("RandomizeSeed".Translate(), null, 1f))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				this.RandomizeSeedAndScenario();
				this.seedIsValid = true;
			}
			if (this.seedIsValid)
			{
				listing_Standard.Label("Seed".Translate().CapitalizeFirst(), -1f, null);
				string a = listing_Standard.TextEntry(this.seed, 1);
				if (a != this.seed)
				{
					this.seed = a;
					this.curScen = ScenarioMaker.GenerateNewRandomScenario(this.seed);
				}
			}
			else
			{
				listing_Standard.Gap(Text.LineHeight + Text.LineHeight + 2f);
			}
			listing_Standard.CheckboxLabeled("EditMode".Translate().CapitalizeFirst(), ref this.editMode, null, 0f, 1f);
			if (this.editMode)
			{
				this.seedIsValid = false;
				if (listing_Standard.ButtonText("AddPart".Translate(), null, 1f))
				{
					this.OpenAddScenPartMenu();
				}
				if (SteamManager.Initialized && (this.curScen.Category == ScenarioCategory.CustomLocal || this.curScen.Category == ScenarioCategory.SteamWorkshop) && listing_Standard.ButtonText(Workshop.UploadButtonLabel(this.curScen.GetPublishedFileId()), null, 1f) && Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
				{
					AcceptanceReport acceptanceReport = this.curScen.TryUploadReport();
					if (!acceptanceReport.Accepted)
					{
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSteamWorkshopUpload".Translate(), delegate()
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmContentAuthor".Translate(), delegate()
							{
								SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
								Workshop.Upload(this.curScen);
							}, true, null, WindowLayer.Dialog));
						}, true, null, WindowLayer.Dialog));
					}
				}
			}
			listing_Standard.End();
		}

		// Token: 0x0600C004 RID: 49156 RVA: 0x0045F350 File Offset: 0x0045D550
		private static bool CheckAllPartsCompatible(Scenario scen)
		{
			foreach (ScenPart scenPart in scen.AllParts)
			{
				int num = 0;
				foreach (ScenPart scenPart2 in scen.AllParts)
				{
					if (scenPart2.def == scenPart.def)
					{
						num++;
					}
					if (num > scenPart.def.maxUses)
					{
						Messages.Message("TooMany".Translate(scenPart.def.maxUses) + ": " + scenPart.def.label, MessageTypeDefOf.RejectInput, false);
						return false;
					}
					if (scenPart != scenPart2 && !scenPart.CanCoexistWith(scenPart2))
					{
						Messages.Message("Incompatible".Translate() + ": " + scenPart.def.label + ", " + scenPart2.def.label, MessageTypeDefOf.RejectInput, false);
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600C005 RID: 49157 RVA: 0x0045F4C8 File Offset: 0x0045D6C8
		private void OpenAddScenPartMenu()
		{
			FloatMenuUtility.MakeMenu<ScenPartDef>(from p in ScenarioMaker.AddableParts(this.curScen)
			where p.PlayerAddRemovable
			orderby p.label
			select p, (ScenPartDef p) => p.LabelCap, (ScenPartDef p) => delegate()
			{
				this.AddScenPart(p);
			});
		}

		// Token: 0x0600C006 RID: 49158 RVA: 0x0045F558 File Offset: 0x0045D758
		private void AddScenPart(ScenPartDef def)
		{
			ScenPart scenPart = ScenarioMaker.MakeScenPart(def);
			scenPart.Randomize();
			this.curScen.parts.Add(scenPart);
		}

		// Token: 0x0600C007 RID: 49159 RVA: 0x0045F584 File Offset: 0x0045D784
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.curScen == null)
			{
				return false;
			}
			if (!Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
			{
				return false;
			}
			if (!this.ScenarioEdited || !this.needsSave)
			{
				Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
				return true;
			}
			Find.WindowStack.Add(new Dialog_MessageBox("ScenarioChangedSavePrompt".Translate(), "Yes".Translate(), delegate()
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Save(this.curScen, delegate()
				{
					Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
					this.needsSave = false;
					this.DoNext();
				}));
			}, "No".Translate(), delegate()
			{
				Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
				this.needsSave = false;
				this.DoNext();
			}, null, false, null, delegate()
			{
			}, WindowLayer.Dialog));
			return false;
		}

		// Token: 0x04006E8C RID: 28300
		private Scenario curScen;

		// Token: 0x04006E8D RID: 28301
		private Vector2 infoScrollPosition = Vector2.zero;

		// Token: 0x04006E8E RID: 28302
		private string seed;

		// Token: 0x04006E8F RID: 28303
		private bool seedIsValid = true;

		// Token: 0x04006E90 RID: 28304
		private bool editMode;

		// Token: 0x04006E91 RID: 28305
		private bool needsSave = true;

		// Token: 0x04006E92 RID: 28306
		private int initialHash;
	}
}
