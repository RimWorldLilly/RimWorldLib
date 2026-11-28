using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200199A RID: 6554
	public class CompStatue : CompArt
	{
		// Token: 0x17001C93 RID: 7315
		// (get) Token: 0x0600A478 RID: 42104 RVA: 0x003AC131 File Offset: 0x003AA331
		public override bool Active
		{
			get
			{
				return this.fakePawn != null;
			}
		}

		// Token: 0x17001C94 RID: 7316
		// (get) Token: 0x0600A479 RID: 42105 RVA: 0x003AC13C File Offset: 0x003AA33C
		public new CompProperties_Statue Props
		{
			get
			{
				return (CompProperties_Statue)this.props;
			}
		}

		// Token: 0x17001C95 RID: 7317
		// (get) Token: 0x0600A47A RID: 42106 RVA: 0x003AC149 File Offset: 0x003AA349
		public Graphic StatueBaseGraphic
		{
			get
			{
				return this.Props.statueBaseGraphic.GraphicColoredFor(this.parent);
			}
		}

		// Token: 0x0600A47B RID: 42107 RVA: 0x003AC164 File Offset: 0x003AA364
		public static List<Pawn> GetAllPossiblePawnsToSelectFrom(Pawn crafter)
		{
			CompStatue.tmpPawnsToSelectFrom.Clear();
			CompStatue.tmpPawnsToSelectFrom.AddRange(SocialCardUtility.PawnsForSocialInfo(crafter));
			CompStatue.tmpPawnsToSelectFrom.AddRange(from p in crafter.relations.RelatedPawns
			where p.RaceProps.Humanlike && !p.Spawned
			select p);
			return CompStatue.tmpPawnsToSelectFrom;
		}

		// Token: 0x0600A47C RID: 42108 RVA: 0x003AC1CC File Offset: 0x003AA3CC
		public static Thing GenerateSpecificStatue(Pawn crafter, Pawn subject)
		{
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Statue, ThingDefOf.Steel);
			CompStatue compStatue = thing.TryGetComp<CompStatue>();
			compStatue.CreateSnapshotOfPawn(subject);
			compStatue.InitFakePawn();
			compStatue.titleInt = compStatue.GenerateTitle(ArtGenerationContext.Colony);
			compStatue.descriptionSeed = Rand.Int;
			subject.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MadeStatueOfMe, crafter, null);
			return thing;
		}

		// Token: 0x0600A47D RID: 42109 RVA: 0x003AC238 File Offset: 0x003AA438
		protected override void InitializeArtInternal(Thing relatedThing, ArtGenerationContext source)
		{
		}

		// Token: 0x0600A47E RID: 42110 RVA: 0x003AC23C File Offset: 0x003AA43C
		private void InitializeAsFactionLeaderStatue()
		{
			List<Pawn> list = Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.FactionLeader).Where(delegate(Pawn p)
			{
				if (p.RaceProps.Humanlike && !p.IsSubhuman)
				{
					Faction faction = p.Faction;
					return faction != null && !faction.temporary && !faction.Hidden;
				}
				return false;
			}).ToList<Pawn>();
			if (list.NullOrEmpty<Pawn>())
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(106, 1);
				defaultInterpolatedStringHandler.AppendLiteral("CompStatue at ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(this.parent.PositionHeld);
				defaultInterpolatedStringHandler.AppendLiteral(" could not find any faction leaders to become a statue of. Destroying. This is likely a bug.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			Pawn p2 = list.RandomElement<Pawn>();
			this.CreateSnapshotOfPawn(p2);
			this.InitFakePawn();
			this.titleInt = this.GenerateTitle(ArtGenerationContext.Outsider);
			this.descriptionSeed = Rand.Int;
		}

		// Token: 0x0600A47F RID: 42111 RVA: 0x003AC304 File Offset: 0x003AA504
		public override void JustCreatedBy(Pawn pawn)
		{
			base.JustCreatedBy(pawn);
			if (!base.CanShowArt)
			{
				return;
			}
			Pawn pawn2 = CompStatue.GetAllPossiblePawnsToSelectFrom(pawn).RandomElementByWeight((Pawn p) => CompStatue.sculptureTargetWeightByOpinionCurve.Evaluate((float)pawn.relations.OpinionOf(p)));
			if (pawn2 == null)
			{
				return;
			}
			Pawn_NeedsTracker needs = pawn2.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.MadeStatueOfMe, pawn, null);
						}
					}
				}
			}
			this.CreateSnapshotOfPawn(pawn2);
			this.InitFakePawn();
			this.titleInt = this.GenerateTitle(ArtGenerationContext.Colony);
			this.descriptionSeed = Rand.Int;
		}

		// Token: 0x0600A480 RID: 42112 RVA: 0x003AC3BC File Offset: 0x003AA5BC
		public override void PostPostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			this.InitializeAsFactionLeaderStatue();
		}

		// Token: 0x0600A481 RID: 42113 RVA: 0x003AC3C4 File Offset: 0x003AA5C4
		private void CreateSnapshotOfPawn(Pawn p)
		{
			this.bodyType = p.story.bodyType;
			this.headType = p.story.headType;
			this.hairDef = p.story.hairDef;
			this.hairColor = p.story.HairColor;
			Pawn_StyleTracker style = p.style;
			this.beard = ((style != null) ? style.beardDef : null);
			this.name = p.Name;
			this.gender = p.gender;
			this.lifestageIndex = p.ageTracker.CurLifeStageIndex;
			this.xenotype = p.genes.Xenotype;
			this.apparel.Clear();
			this.apparelStyles.Clear();
			int num = 0;
			foreach (Apparel apparel in p.apparel.WornApparel)
			{
				if (!apparel.def.thingCategories.NotNullAndContains(ThingCategoryDefOf.ApparelUtility) && (!PawnApparelGenerator.IsHeadgear(apparel.def) || Rand.Bool))
				{
					this.apparel.Add(apparel.def);
					if (apparel.StyleDef != null)
					{
						this.apparelStyles.Add(num, apparel.StyleDef);
					}
					num++;
				}
			}
			this.nonXenotypeGenes.Clear();
			if (ModsConfig.BiotechActive)
			{
				foreach (Gene gene in p.genes.GenesListForReading)
				{
					if (!this.xenotype.genes.Contains(gene.def))
					{
						this.nonXenotypeGenes.Add(gene.def);
					}
				}
			}
			this.hediffsWhichAffectRendering.Clear();
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				if (hediff.def.HasDefinedGraphicProperties)
				{
					this.hediffsWhichAffectRendering.Add(new CompStatue.SavedHediffProps(hediff.Part, hediff.def, hediff.Severity));
				}
			}
			this.additionalSavedPawnDataForMods.Clear();
			this.CreateSnapshotOfPawn_HookForMods(p, this.additionalSavedPawnDataForMods);
		}

		// Token: 0x0600A482 RID: 42114 RVA: 0x003AC638 File Offset: 0x003AA838
		[UsedImplicitly]
		private void CreateSnapshotOfPawn_HookForMods(Pawn p, Dictionary<string, object> dictToStoreDataIn)
		{
		}

		// Token: 0x0600A483 RID: 42115 RVA: 0x003AC63C File Offset: 0x003AA83C
		private void InitFakePawn()
		{
			this.fakePawn = (Pawn)ThingMaker.MakeThing(ThingDefOf.Human, null);
			PawnComponentsUtility.CreateInitialComponents(this.fakePawn);
			this.fakePawn.Name = this.name;
			this.fakePawn.gender = this.gender;
			this.fakePawn.kindDef = PawnKindDefOf.Colonist;
			if (ModsConfig.BiotechActive)
			{
				this.fakePawn.genes.SetXenotype(this.xenotype ?? XenotypeDefOf.Baseliner);
			}
			this.fakePawn.ageTracker.LockCurrentLifeStageIndex(this.lifestageIndex);
			this.fakePawn.story.bodyType = this.bodyType;
			this.fakePawn.story.headType = this.headType;
			this.fakePawn.story.hairDef = this.hairDef;
			this.fakePawn.story.HairColor = this.hairColor;
			this.fakePawn.style.beardDef = (this.beard ?? BeardDefOf.NoBeard);
			this.fakePawn.apparel.DestroyAll(DestroyMode.Vanish);
			for (int i = 0; i < this.apparel.Count; i++)
			{
				ThingDef thingDef = this.apparel[i];
				ThingStyleDef thingStyleDef;
				this.apparelStyles.TryGetValue(i, out thingStyleDef);
				Apparel apparel;
				if (thingDef.MadeFromStuff)
				{
					apparel = (Apparel)ThingMaker.MakeThing(thingDef, GenStuff.DefaultStuffFor(thingDef));
				}
				else
				{
					apparel = (Apparel)ThingMaker.MakeThing(thingDef, null);
				}
				if (thingStyleDef != null)
				{
					apparel.StyleDef = thingStyleDef;
				}
				this.fakePawn.apparel.WornApparel.Add(apparel);
			}
			if (!this.nonXenotypeGenes.NullOrEmpty<GeneDef>())
			{
				foreach (GeneDef geneDef in this.nonXenotypeGenes)
				{
					this.fakePawn.genes.AddGene(geneDef, true);
				}
			}
			if (!this.hediffsWhichAffectRendering.NullOrEmpty<CompStatue.SavedHediffProps>())
			{
				foreach (CompStatue.SavedHediffProps savedHediffProps in this.hediffsWhichAffectRendering)
				{
					this.fakePawn.health.AddHediff(savedHediffProps.hediffDef, savedHediffProps.bodyPart, null, null).Severity = savedHediffProps.severity;
				}
			}
			this.InitFakePawn_HookForMods(this.fakePawn, this.additionalSavedPawnDataForMods);
			this.fakePawn.Drawer.renderer.SetStatue(this.parent.Stuff);
			this.fakePawn.Drawer.renderer.SetAllGraphicsDirty();
			this.fakePawn.Drawer.renderer.EnsureGraphicsInitialized();
			this.Notify_ColorChanged();
		}

		// Token: 0x0600A484 RID: 42116 RVA: 0x003AC920 File Offset: 0x003AAB20
		[UsedImplicitly]
		private void InitFakePawn_HookForMods(Pawn fakePawn, Dictionary<string, object> additionalSavedPawnDataForMods)
		{
		}

		// Token: 0x0600A485 RID: 42117 RVA: 0x003AC924 File Offset: 0x003AAB24
		public override TaggedString GenerateImageDescription()
		{
			if (this.fakePawn == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(147, 1);
				defaultInterpolatedStringHandler.AppendLiteral("CompStatue at ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(this.parent.PositionHeld);
				defaultInterpolatedStringHandler.AppendLiteral(" trying to GenerateImageDescription before initializing pawn. This will not work. This message will not appear again for this statue.");
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), this.parent.thingIDNumber ^ 61006258);
				return null;
			}
			GrammarRequest grammarRequest = default(GrammarRequest);
			grammarRequest.Includes.Add(this.Props.descriptionMaker);
			grammarRequest.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.fakePawn, grammarRequest.Constants, true, true));
			Rand.PushState();
			Rand.Seed = this.descriptionSeed;
			string rootKeyword = "r_statue_description";
			GrammarRequest request = grammarRequest;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(19, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("statue_description_");
			defaultInterpolatedStringHandler2.AppendFormatted<Name>(this.name);
			string str = GrammarResolver.Resolve(rootKeyword, request, defaultInterpolatedStringHandler2.ToStringAndClear(), false, null, null, null, true);
			Rand.PopState();
			return str;
		}

		// Token: 0x0600A486 RID: 42118 RVA: 0x003ACA2C File Offset: 0x003AAC2C
		protected override string GenerateTitle(ArtGenerationContext context)
		{
			GrammarRequest grammarRequest = default(GrammarRequest);
			grammarRequest.Includes.Add(this.Props.nameMaker);
			grammarRequest.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.fakePawn, grammarRequest.Constants, true, true));
			string rootKeyword = "r_statue_name";
			GrammarRequest request = grammarRequest;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
			defaultInterpolatedStringHandler.AppendLiteral("statue_name_");
			defaultInterpolatedStringHandler.AppendFormatted<Name>(this.name);
			return GrammarResolver.Resolve(rootKeyword, request, defaultInterpolatedStringHandler.ToStringAndClear(), false, null, null, null, true);
		}

		// Token: 0x0600A487 RID: 42119 RVA: 0x003ACAB8 File Offset: 0x003AACB8
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (text == null)
			{
				return null;
			}
			string str = text;
			string str2 = "\n" + "Subject".Translate() + ": ";
			Name name = this.name;
			return str + str2 + ((name != null) ? name.ToString() : null);
		}

		// Token: 0x0600A488 RID: 42120 RVA: 0x003ACB0E File Offset: 0x003AAD0E
		public override bool DontDrawParent()
		{
			return true;
		}

		// Token: 0x0600A489 RID: 42121 RVA: 0x003ACB14 File Offset: 0x003AAD14
		public override void DrawAt(Vector3 drawPos, bool flip = false)
		{
			Vector3 loc = new Vector3(drawPos.x, drawPos.y - 0.03658537f, drawPos.z - 0.15f);
			this.StatueBaseGraphic.Draw(loc, flip ? this.parent.Rotation.Opposite : this.parent.Rotation, this.parent, 0f);
			if (this.fakePawn == null)
			{
				if (this.name != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(184, 1);
					defaultInterpolatedStringHandler.AppendLiteral("CompStatue at ");
					defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(this.parent.PositionHeld);
					defaultInterpolatedStringHandler.AppendLiteral(" has chosen a target pawn but didn't successfully initialize for rendering. This is a bug. The statue will not render. This message will not appear again for this statue.");
					Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), this.parent.thingIDNumber ^ 61010354);
					return;
				}
				this.InitializeAsFactionLeaderStatue();
			}
			float valueOrDefault = this.fakePawn.ageTracker.CurLifeStage.headSizeFactor.GetValueOrDefault(1f);
			float d = (1f - valueOrDefault) * 0.5f;
			drawPos += Vector3.back * d;
			drawPos -= Altitudes.AltIncVect * 0.25f;
			this.fakePawn.Drawer.renderer.RenderPawnAt(drawPos, new Rot4?(Rot4.South), true);
		}

		// Token: 0x0600A48A RID: 42122 RVA: 0x003ACC64 File Offset: 0x003AAE64
		public override void Notify_ColorChanged()
		{
			if (this.fakePawn == null)
			{
				return;
			}
			this.fakePawn.Drawer.renderer.SetStatue(this.parent.Stuff);
			Building building = this.parent as Building;
			if (building == null || building.PaintColorDef == null)
			{
				this.fakePawn.Drawer.renderer.SetStatuePaintColor(null);
			}
			else
			{
				this.fakePawn.Drawer.renderer.SetStatuePaintColor(new Color?(building.PaintColorDef.color));
			}
			this.fakePawn.Drawer.renderer.SetAllGraphicsDirty();
			this.fakePawn.Drawer.renderer.EnsureGraphicsInitialized();
		}

		// Token: 0x0600A48B RID: 42123 RVA: 0x003ACD20 File Offset: 0x003AAF20
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<BodyTypeDef>(ref this.bodyType, "bodyType");
			Scribe_Defs.Look<HeadTypeDef>(ref this.headType, "headType");
			Scribe_Defs.Look<HairDef>(ref this.hairDef, "hairDef");
			Scribe_Defs.Look<XenotypeDef>(ref this.xenotype, "xenotype");
			Scribe_Defs.Look<BeardDef>(ref this.beard, "beard");
			Scribe_Collections.Look<ThingDef>(ref this.apparel, "apparel", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<int, ThingStyleDef>(ref this.apparelStyles, "apparelStyles", LookMode.Value, LookMode.Def);
			Scribe_Deep.Look<Name>(ref this.name, "name", Array.Empty<object>());
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<int>(ref this.lifestageIndex, "lifestageIndex", 0, false);
			Scribe_Values.Look<Color>(ref this.hairColor, "hairColor", default(Color), false);
			Scribe_Values.Look<int>(ref this.descriptionSeed, "descriptionSeed", 0, false);
			Scribe_Collections.Look<GeneDef>(ref this.nonXenotypeGenes, "nonXenotypeGenes", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<CompStatue.SavedHediffProps>(ref this.hediffsWhichAffectRendering, "hediffsWhichAffectRendering", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string, object>(ref this.additionalSavedPawnDataForMods, "additionalSavedPawnDataForMods", LookMode.Value, LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.descriptionSeed == 0)
				{
					this.descriptionSeed = Rand.Int;
				}
				if (this.additionalSavedPawnDataForMods == null)
				{
					this.additionalSavedPawnDataForMods = new Dictionary<string, object>();
				}
				if (this.apparelStyles == null)
				{
					this.apparelStyles = new Dictionary<int, ThingStyleDef>();
				}
				LongEventHandler.ExecuteWhenFinished(new Action(this.InitFakePawn));
			}
		}

		// Token: 0x04006137 RID: 24887
		private static readonly SimpleCurve sculptureTargetWeightByOpinionCurve = new SimpleCurve(new CurvePoint[]
		{
			new CurvePoint(-100f, 1f),
			new CurvePoint(0f, 2f),
			new CurvePoint(100f, 10f)
		});

		// Token: 0x04006138 RID: 24888
		private static List<Pawn> tmpPawnsToSelectFrom = new List<Pawn>();

		// Token: 0x04006139 RID: 24889
		private Pawn fakePawn;

		// Token: 0x0400613A RID: 24890
		private BodyTypeDef bodyType;

		// Token: 0x0400613B RID: 24891
		private HeadTypeDef headType;

		// Token: 0x0400613C RID: 24892
		private HairDef hairDef;

		// Token: 0x0400613D RID: 24893
		private XenotypeDef xenotype;

		// Token: 0x0400613E RID: 24894
		public BeardDef beard;

		// Token: 0x0400613F RID: 24895
		private Color hairColor;

		// Token: 0x04006140 RID: 24896
		private List<ThingDef> apparel = new List<ThingDef>();

		// Token: 0x04006141 RID: 24897
		private Dictionary<int, ThingStyleDef> apparelStyles = new Dictionary<int, ThingStyleDef>();

		// Token: 0x04006142 RID: 24898
		private List<GeneDef> nonXenotypeGenes = new List<GeneDef>();

		// Token: 0x04006143 RID: 24899
		private List<CompStatue.SavedHediffProps> hediffsWhichAffectRendering = new List<CompStatue.SavedHediffProps>();

		// Token: 0x04006144 RID: 24900
		private Name name;

		// Token: 0x04006145 RID: 24901
		private Gender gender;

		// Token: 0x04006146 RID: 24902
		private int lifestageIndex;

		// Token: 0x04006147 RID: 24903
		private int descriptionSeed;

		// Token: 0x04006148 RID: 24904
		private Dictionary<string, object> additionalSavedPawnDataForMods = new Dictionary<string, object>();

		// Token: 0x020036E7 RID: 14055
		public class SavedHediffProps : IExposable
		{
			// Token: 0x060140F6 RID: 82166 RVA: 0x0067B131 File Offset: 0x00679331
			public SavedHediffProps(BodyPartRecord bodyPart, HediffDef hediffDef, float severity)
			{
				this.bodyPart = bodyPart;
				this.hediffDef = hediffDef;
				this.severity = severity;
			}

			// Token: 0x060140F7 RID: 82167 RVA: 0x0067B14E File Offset: 0x0067934E
			public SavedHediffProps()
			{
			}

			// Token: 0x060140F8 RID: 82168 RVA: 0x0067B156 File Offset: 0x00679356
			public void ExposeData()
			{
				Scribe_BodyParts.Look(ref this.bodyPart, "bodyPart", null);
				Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
				Scribe_Values.Look<float>(ref this.severity, "severity", 0f, false);
			}

			// Token: 0x0400DB35 RID: 56117
			public BodyPartRecord bodyPart;

			// Token: 0x0400DB36 RID: 56118
			public HediffDef hediffDef;

			// Token: 0x0400DB37 RID: 56119
			public float severity;
		}
	}
}
