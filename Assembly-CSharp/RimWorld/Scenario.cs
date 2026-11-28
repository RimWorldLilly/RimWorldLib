using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Steamworks;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001785 RID: 6021
	public class Scenario : IExposable, WorkshopUploadable
	{
		// Token: 0x17001992 RID: 6546
		// (get) Token: 0x060095CC RID: 38348 RVA: 0x00367123 File Offset: 0x00365323
		public IEnumerable<Version> SupportedVersions
		{
			get
			{
				return new Scenario.<get_SupportedVersions>d__16(-2);
			}
		}

		// Token: 0x17001993 RID: 6547
		// (get) Token: 0x060095CD RID: 38349 RVA: 0x0036712C File Offset: 0x0036532C
		public FileInfo File
		{
			get
			{
				return new FileInfo(GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x17001994 RID: 6548
		// (get) Token: 0x060095CE RID: 38350 RVA: 0x0036713E File Offset: 0x0036533E
		public IEnumerable<ScenPart> AllParts
		{
			get
			{
				Scenario.<get_AllParts>d__23 <get_AllParts>d__ = new Scenario.<get_AllParts>d__23(-2);
				<get_AllParts>d__.<>4__this = this;
				return <get_AllParts>d__;
			}
		}

		// Token: 0x17001995 RID: 6549
		// (get) Token: 0x060095CF RID: 38351 RVA: 0x0036714E File Offset: 0x0036534E
		// (set) Token: 0x060095D0 RID: 38352 RVA: 0x0036717A File Offset: 0x0036537A
		public ScenarioCategory Category
		{
			get
			{
				if (this.categoryInt == ScenarioCategory.Undefined)
				{
					Log.Error("Category is Undefined on Scenario " + ((this != null) ? this.ToString() : null));
				}
				return this.categoryInt;
			}
			set
			{
				this.categoryInt = value;
			}
		}

		// Token: 0x17001996 RID: 6550
		// (get) Token: 0x060095D1 RID: 38353 RVA: 0x00367184 File Offset: 0x00365384
		public bool OverrideDangerMusic
		{
			get
			{
				using (IEnumerator<ScenPart> enumerator = this.AllParts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.OverrideDangerMusic)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x060095D3 RID: 38355 RVA: 0x0036720C File Offset: 0x0036540C
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<string>(ref this.summary, "summary", null, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
			Scribe_Values.Look<PublishedFileId_t>(ref this.publishedFileIdInt, "publishedFileId", PublishedFileId_t.Invalid, false);
			Scribe_Deep.Look<ScenPart_PlayerFaction>(ref this.playerFaction, "playerFaction", Array.Empty<object>());
			Scribe_Deep.Look<ScenPart_PlanetLayer>(ref this.surfaceLayer, "surfaceLayer", Array.Empty<object>());
			Scribe_Collections.Look<ScenPart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.surfaceLayer == null)
				{
					this.surfaceLayer = new ScenPart_PlanetLayer
					{
						def = ScenPartDefOf.PlanetLayerFixed,
						layer = PlanetLayerDefOf.Surface,
						settingsDef = PlanetLayerSettingsDefOf.Surface,
						hide = true,
						tag = "Surface"
					};
				}
				if (ModsConfig.OdysseyActive)
				{
					if (!this.parts.Any(delegate(ScenPart p)
					{
						ScenPart_PlanetLayer scenPart_PlanetLayer2 = p as ScenPart_PlanetLayer;
						return scenPart_PlanetLayer2 != null && scenPart_PlanetLayer2.tag == "Orbit";
					}))
					{
						ScenPart_PlanetLayer scenPart_PlanetLayer = new ScenPart_PlanetLayer
						{
							def = ScenPartDefOf.PlanetLayerFixed,
							layer = PlanetLayerDefOf.Orbit,
							settingsDef = PlanetLayerSettingsDefOf.Orbit,
							hide = true,
							tag = "Orbit"
						};
						this.parts.Add(scenPart_PlanetLayer);
						this.surfaceLayer.connections.Add(new LayerConnection
						{
							tag = scenPart_PlanetLayer.tag,
							zoomMode = LayerConnection.ZoomMode.ZoomOut
						});
						scenPart_PlanetLayer.connections.Add(new LayerConnection
						{
							tag = this.surfaceLayer.tag,
							zoomMode = LayerConnection.ZoomMode.ZoomIn
						});
					}
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.parts.RemoveAll((ScenPart p) => p == null || !p.Valid()) != 0)
				{
					Log.Warning("Some scenario parts were null or invalid after loading");
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060095D4 RID: 38356 RVA: 0x00367408 File Offset: 0x00365608
		public IEnumerable<string> ConfigErrors()
		{
			Scenario.<ConfigErrors>d__31 <ConfigErrors>d__ = new Scenario.<ConfigErrors>d__31(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x060095D5 RID: 38357 RVA: 0x00367418 File Offset: 0x00365618
		public string GetFullInformationText()
		{
			string result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.description);
				stringBuilder.AppendLine();
				foreach (ScenPart scenPart in this.AllParts)
				{
					scenPart.summarized = false;
				}
				foreach (ScenPart scenPart2 in from p in this.AllParts
				orderby p.def.summaryPriority descending, p.def.defName
				where p.visible
				select p)
				{
					string text = scenPart2.Summary(this);
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text);
					}
				}
				result = stringBuilder.ToString().TrimEndNewlines();
			}
			catch (Exception ex)
			{
				Log.ErrorOnce("Exception in Scenario.GetFullInformationText():\n" + ex.ToString(), 10395878);
				result = "Cannot read data.";
			}
			return result;
		}

		// Token: 0x060095D6 RID: 38358 RVA: 0x00367598 File Offset: 0x00365798
		public string GetSummary()
		{
			return this.summary;
		}

		// Token: 0x060095D7 RID: 38359 RVA: 0x003675A0 File Offset: 0x003657A0
		public Scenario CopyForEditing()
		{
			Scenario scenario = new Scenario();
			scenario.name = this.name;
			scenario.summary = this.summary;
			scenario.description = this.description;
			scenario.playerFaction = (ScenPart_PlayerFaction)this.playerFaction.CopyForEditing();
			scenario.surfaceLayer = (ScenPart_PlanetLayer)this.surfaceLayer.CopyForEditing();
			scenario.parts.AddRange(from p in this.parts
			select p.CopyForEditing());
			scenario.categoryInt = ScenarioCategory.CustomLocal;
			return scenario;
		}

		// Token: 0x060095D8 RID: 38360 RVA: 0x00367640 File Offset: 0x00365840
		public void PreConfigure()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreConfigure();
			}
		}

		// Token: 0x060095D9 RID: 38361 RVA: 0x0036768C File Offset: 0x0036588C
		public virtual Page GetFirstConfigPage()
		{
			List<Page> list = new List<Page>();
			list.Add(new Page_SelectStoryteller());
			list.Add(new Page_CreateWorldParams());
			if (!this.parts.Any((ScenPart p) => p is ScenPart_ForcedMap))
			{
				list.Add(new Page_SelectStartingSite());
			}
			if (ModsConfig.IdeologyActive)
			{
				list.Add(new Page_ChooseIdeoPreset());
			}
			foreach (Page item in this.parts.SelectMany((ScenPart p) => p.GetConfigPages()))
			{
				list.Add(item);
			}
			Page page = PageUtility.StitchedPages(list);
			if (page != null)
			{
				Page page2 = page;
				while (page2.next != null)
				{
					page2 = page2.next;
				}
				page2.nextAct = new Action(PageUtility.InitGameStart);
			}
			return page;
		}

		// Token: 0x060095DA RID: 38362 RVA: 0x00367798 File Offset: 0x00365998
		public bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			using (IEnumerator<ScenPart> enumerator = this.AllParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060095DB RID: 38363 RVA: 0x003677F0 File Offset: 0x003659F0
		public void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_NewPawnGenerating(pawn, context);
			}
		}

		// Token: 0x060095DC RID: 38364 RVA: 0x0036783C File Offset: 0x00365A3C
		public void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_PawnGenerated(pawn, context, redressed);
			}
		}

		// Token: 0x060095DD RID: 38365 RVA: 0x0036788C File Offset: 0x00365A8C
		public void Notify_PawnDied(Corpse corpse)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnDied(corpse);
			}
		}

		// Token: 0x060095DE RID: 38366 RVA: 0x003678C4 File Offset: 0x00365AC4
		public void PostWorldGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostWorldGenerate();
			}
		}

		// Token: 0x060095DF RID: 38367 RVA: 0x00367910 File Offset: 0x00365B10
		public void PostIdeoChosen()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostIdeoChosen();
			}
		}

		// Token: 0x060095E0 RID: 38368 RVA: 0x0036795C File Offset: 0x00365B5C
		public void PreMapGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreMapGenerate();
			}
		}

		// Token: 0x060095E1 RID: 38369 RVA: 0x003679A8 File Offset: 0x00365BA8
		public void GenerateIntoMap(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.GenerateIntoMap(map);
			}
		}

		// Token: 0x060095E2 RID: 38370 RVA: 0x003679F4 File Offset: 0x00365BF4
		public void PostMapGenerate(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostMapGenerate(map);
			}
		}

		// Token: 0x060095E3 RID: 38371 RVA: 0x00367A40 File Offset: 0x00365C40
		public void PostGravshipLanded(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostGravshipLanded(map);
			}
		}

		// Token: 0x060095E4 RID: 38372 RVA: 0x00367A8C File Offset: 0x00365C8C
		public void MapRemoved(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.MapRemoved(map);
			}
		}

		// Token: 0x060095E5 RID: 38373 RVA: 0x00367AD8 File Offset: 0x00365CD8
		public void PostGameStart()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostGameStart();
			}
		}

		// Token: 0x060095E6 RID: 38374 RVA: 0x00367B24 File Offset: 0x00365D24
		public float GetStatFactor(StatDef stat)
		{
			float num = 1f;
			for (int i = 0; i < this.parts.Count; i++)
			{
				ScenPart_StatFactor scenPart_StatFactor = this.parts[i] as ScenPart_StatFactor;
				if (scenPart_StatFactor != null)
				{
					num *= scenPart_StatFactor.GetStatFactor(stat);
				}
			}
			return num;
		}

		// Token: 0x060095E7 RID: 38375 RVA: 0x00367B70 File Offset: 0x00365D70
		public void TickScenario()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Tick();
			}
		}

		// Token: 0x060095E8 RID: 38376 RVA: 0x00367BA4 File Offset: 0x00365DA4
		public void RemovePart(ScenPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Cannot remove: " + ((part != null) ? part.ToString() : null));
			}
			this.parts.Remove(part);
		}

		// Token: 0x060095E9 RID: 38377 RVA: 0x00367BE0 File Offset: 0x00365DE0
		public bool CanReorder(ScenPart part, ReorderDirection dir)
		{
			if (!part.def.PlayerAddRemovable)
			{
				return false;
			}
			int num = this.parts.IndexOf(part);
			if (dir == ReorderDirection.Up)
			{
				return num != 0 && (num <= 0 || this.parts[num - 1].def.PlayerAddRemovable);
			}
			if (dir == ReorderDirection.Down)
			{
				return num != this.parts.Count - 1;
			}
			throw new NotImplementedException();
		}

		// Token: 0x060095EA RID: 38378 RVA: 0x00367C50 File Offset: 0x00365E50
		public void Reorder(ScenPart part, ReorderDirection dir)
		{
			int num = this.parts.IndexOf(part);
			this.parts.RemoveAt(num);
			if (dir == ReorderDirection.Up)
			{
				this.parts.Insert(num - 1, part);
			}
			if (dir == ReorderDirection.Down)
			{
				this.parts.Insert(num + 1, part);
			}
		}

		// Token: 0x060095EB RID: 38379 RVA: 0x00367C9C File Offset: 0x00365E9C
		public bool CanToUploadToWorkshop()
		{
			return this.Category != ScenarioCategory.FromDef && this.TryUploadReport().Accepted && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x060095EC RID: 38380 RVA: 0x00367CD8 File Offset: 0x00365ED8
		public void PrepareForWorkshopUpload()
		{
			string path = GenFile.SanitizedFileName(this.name) + Rand.RangeInclusive(100, 999).ToString();
			this.tempUploadDir = Path.Combine(GenFilePaths.TempFolderPath, path);
			DirectoryInfo directoryInfo = new DirectoryInfo(this.tempUploadDir);
			if (directoryInfo.Exists)
			{
				directoryInfo.Delete();
			}
			directoryInfo.Create();
			string text = Path.Combine(this.tempUploadDir, GenFile.SanitizedFileName(this.name));
			text += ".rsc";
			GameDataSaveLoader.SaveScenario(this, text);
		}

		// Token: 0x060095ED RID: 38381 RVA: 0x00367D68 File Offset: 0x00365F68
		public AcceptanceReport TryUploadReport()
		{
			if (this.name == null || this.name.Length < 3 || this.summary == null || this.summary.Length < 3 || this.description == null || this.description.Length < 3)
			{
				return "TextFieldsMustBeFilled".TranslateSimple();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060095EE RID: 38382 RVA: 0x00367DCC File Offset: 0x00365FCC
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x060095EF RID: 38383 RVA: 0x00367DD4 File Offset: 0x00365FD4
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			this.publishedFileIdInt = newPfid;
			if (this.Category == ScenarioCategory.CustomLocal && !this.fileName.NullOrEmpty())
			{
				GameDataSaveLoader.SaveScenario(this, GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x060095F0 RID: 38384 RVA: 0x00367E04 File Offset: 0x00366004
		public string GetWorkshopName()
		{
			return this.name;
		}

		// Token: 0x060095F1 RID: 38385 RVA: 0x00367E0C File Offset: 0x0036600C
		public string GetWorkshopDescription()
		{
			return this.GetFullInformationText();
		}

		// Token: 0x060095F2 RID: 38386 RVA: 0x00367E14 File Offset: 0x00366014
		public string GetWorkshopPreviewImagePath()
		{
			return GenFilePaths.ScenarioPreviewImagePath;
		}

		// Token: 0x060095F3 RID: 38387 RVA: 0x00367E1B File Offset: 0x0036601B
		public IList<string> GetWorkshopTags()
		{
			return new List<string>
			{
				"Scenario"
			};
		}

		// Token: 0x060095F4 RID: 38388 RVA: 0x00367E2D File Offset: 0x0036602D
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return new DirectoryInfo(this.tempUploadDir);
		}

		// Token: 0x060095F5 RID: 38389 RVA: 0x00367E3A File Offset: 0x0036603A
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x060095F6 RID: 38390 RVA: 0x00367E56 File Offset: 0x00366056
		public override string ToString()
		{
			if (this.name.NullOrEmpty())
			{
				return "LabellessScenario";
			}
			return this.name;
		}

		// Token: 0x060095F7 RID: 38391 RVA: 0x00367E74 File Offset: 0x00366074
		public override int GetHashCode()
		{
			int num = 6126121;
			if (this.name != null)
			{
				num ^= this.name.GetHashCode();
			}
			if (this.summary != null)
			{
				num ^= this.summary.GetHashCode();
			}
			if (this.description != null)
			{
				num ^= this.description.GetHashCode();
			}
			num ^= this.publishedFileIdInt.GetHashCode();
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (this.parts[i] != null)
				{
					num ^= this.parts[i].GetHashCode() + i;
				}
			}
			return num;
		}

		// Token: 0x04005A20 RID: 23072
		[MustTranslate]
		public string name;

		// Token: 0x04005A21 RID: 23073
		[MustTranslate]
		public string summary;

		// Token: 0x04005A22 RID: 23074
		[MustTranslate]
		public string description;

		// Token: 0x04005A23 RID: 23075
		internal ScenPart_PlayerFaction playerFaction;

		// Token: 0x04005A24 RID: 23076
		internal ScenPart_PlanetLayer surfaceLayer;

		// Token: 0x04005A25 RID: 23077
		internal List<ScenPart> parts = new List<ScenPart>();

		// Token: 0x04005A26 RID: 23078
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x04005A27 RID: 23079
		private ScenarioCategory categoryInt;

		// Token: 0x04005A28 RID: 23080
		[NoTranslate]
		public string fileName;

		// Token: 0x04005A29 RID: 23081
		private WorkshopItemHook workshopHookInt;

		// Token: 0x04005A2A RID: 23082
		[NoTranslate]
		private string tempUploadDir;

		// Token: 0x04005A2B RID: 23083
		public bool enabled = true;

		// Token: 0x04005A2C RID: 23084
		public bool showInUI = true;

		// Token: 0x04005A2D RID: 23085
		public bool valid = true;

		// Token: 0x04005A2E RID: 23086
		public bool standardAnomalyPlaystyleOnly;

		// Token: 0x04005A2F RID: 23087
		public const int NameMaxLength = 55;

		// Token: 0x04005A30 RID: 23088
		public const int SummaryMaxLength = 300;

		// Token: 0x04005A31 RID: 23089
		public const int DescriptionMaxLength = 1000;
	}
}
