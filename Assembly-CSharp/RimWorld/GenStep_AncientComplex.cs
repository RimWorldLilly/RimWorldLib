using System;
using System.Runtime.CompilerServices;
using RimWorld.BaseGen;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E6 RID: 4838
	public class GenStep_AncientComplex : GenStep_LargeRuins
	{
		// Token: 0x17001471 RID: 5233
		// (get) Token: 0x06007805 RID: 30725 RVA: 0x002A7662 File Offset: 0x002A5862
		public override int SeedPart
		{
			get
			{
				return 235635649;
			}
		}

		// Token: 0x17001472 RID: 5234
		// (get) Token: 0x06007806 RID: 30726 RVA: 0x002A7669 File Offset: 0x002A5869
		private IntVec2 Size
		{
			get
			{
				return new IntVec2(this.structureSketch.structureLayout.container.Width + 10, this.structureSketch.structureLayout.container.Height + 10);
			}
		}

		// Token: 0x17001473 RID: 5235
		// (get) Token: 0x06007807 RID: 30727 RVA: 0x002A76A0 File Offset: 0x002A58A0
		protected override LayoutDef LayoutDef
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001474 RID: 5236
		// (get) Token: 0x06007808 RID: 30728 RVA: 0x002A76A3 File Offset: 0x002A58A3
		protected override Faction Faction
		{
			get
			{
				return Faction.OfAncients;
			}
		}

		// Token: 0x17001475 RID: 5237
		// (get) Token: 0x06007809 RID: 30729 RVA: 0x002A76AA File Offset: 0x002A58AA
		protected override IntRange RuinsMinMaxRange
		{
			get
			{
				return IntRange.One;
			}
		}

		// Token: 0x17001476 RID: 5238
		// (get) Token: 0x0600780A RID: 30730 RVA: 0x002A76B1 File Offset: 0x002A58B1
		protected override TerrainAffordanceDef MinAffordance
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001477 RID: 5239
		// (get) Token: 0x0600780B RID: 30731 RVA: 0x002A76B4 File Offset: 0x002A58B4
		protected override bool AvoidWaterRoads
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001478 RID: 5240
		// (get) Token: 0x0600780C RID: 30732 RVA: 0x002A76B8 File Offset: 0x002A58B8
		protected override IntVec2 MinSize
		{
			get
			{
				IntVec2? intVec = this.actualMin;
				if (intVec == null)
				{
					return GenStep_AncientComplex.DefaultComplexSize;
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x17001479 RID: 5241
		// (get) Token: 0x0600780D RID: 30733 RVA: 0x002A76E4 File Offset: 0x002A58E4
		protected override IntVec2 MaxSize
		{
			get
			{
				IntVec2? intVec = this.actualMax;
				if (intVec == null)
				{
					return new IntVec2(100, 100);
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x0600780E RID: 30734 RVA: 0x002A7714 File Offset: 0x002A5914
		public override void Generate(Map map, GenStepParams parms)
		{
			this.structureSketch = parms.sitePart.parms.ancientLayoutStructureSketch;
			parms.sitePart.parms.ancientLayoutStructureSketch = null;
			LayoutStructureSketch layoutStructureSketch = this.structureSketch;
			if (((layoutStructureSketch != null) ? layoutStructureSketch.structureLayout : null) == null)
			{
				this.TryRecoverEmptySketch(parms);
			}
			map.layoutStructureSketches.Add(this.structureSketch);
			this.actualMin = (this.actualMax = new IntVec2?(this.Size));
			base.Generate(map, parms);
			this.actualMin = (this.actualMax = null);
		}

		// Token: 0x0600780F RID: 30735 RVA: 0x002A77B0 File Offset: 0x002A59B0
		protected override LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			CellRect container = this.structureSketch.structureLayout.container;
			CellRect rect2;
			if (!rect.TryFindRandomInnerRect(new IntVec2(container.Width, container.Height), out rect2, null))
			{
				rect2 = rect;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(112, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to generate and spawn an anicent complex, but could not find rect of valid size ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec2>(this.Size);
				defaultInterpolatedStringHandler.AppendLiteral(" within provided Rect ");
				defaultInterpolatedStringHandler.AppendFormatted<CellRect>(rect);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ResolveParams resolveParams = new ResolveParams
			{
				ancientLayoutStructureSketch = this.structureSketch,
				threatPoints = new float?(parms.sitePart.parms.threatPoints),
				rect = rect2,
				thingSetMakerDef = parms.sitePart.parms.ancientComplexRewardMaker
			};
			FormCaravanComp component = parms.sitePart.site.GetComponent<FormCaravanComp>();
			if (component != null)
			{
				component.foggedRoomsCheckRect = new CellRect?(resolveParams.rect);
			}
			MapGenerator.UsedRects.Add(resolveParams.rect);
			this.GenerateComplex(map, resolveParams);
			return this.structureSketch;
		}

		// Token: 0x06007810 RID: 30736 RVA: 0x002A78C8 File Offset: 0x002A5AC8
		private void TryRecoverEmptySketch(GenStepParams parms)
		{
			bool flag = false;
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				QuestPart_SpawnWorldObject questPart_SpawnWorldObject;
				if (quest.TryGetFirstPartOfType<QuestPart_SpawnWorldObject>(out questPart_SpawnWorldObject) && questPart_SpawnWorldObject.worldObject == parms.sitePart.site)
				{
					QuestNode_Root_AncientComplex questNode_Root_AncientComplex = quest.root.root as QuestNode_Root_AncientComplex;
					if (questNode_Root_AncientComplex != null)
					{
						this.structureSketch = questNode_Root_AncientComplex.QuestSetupComplex(quest, parms.sitePart.parms.points);
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return;
			}
			StructureGenParams parms2 = new StructureGenParams
			{
				size = GenStep_AncientComplex.DefaultComplexSize
			};
			this.structureSketch = LayoutDefOf.AncientComplex.Worker.GenerateStructureSketch(parms2);
			Log.Warning("Failed to recover lost complex from any quest. Generating default.");
		}

		// Token: 0x06007811 RID: 30737 RVA: 0x002A79A8 File Offset: 0x002A5BA8
		protected virtual void GenerateComplex(Map map, ResolveParams parms)
		{
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientComplex", parms, null);
			BaseGen.Generate();
		}

		// Token: 0x04004B4F RID: 19279
		private LayoutStructureSketch structureSketch;

		// Token: 0x04004B50 RID: 19280
		private static readonly IntVec2 DefaultComplexSize = new IntVec2(80, 80);

		// Token: 0x04004B51 RID: 19281
		private IntVec2? actualMin;

		// Token: 0x04004B52 RID: 19282
		private IntVec2? actualMax;
	}
}
