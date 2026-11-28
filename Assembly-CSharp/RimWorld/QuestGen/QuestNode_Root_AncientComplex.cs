using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200225E RID: 8798
	public abstract class QuestNode_Root_AncientComplex : QuestNode
	{
		// Token: 0x17002609 RID: 9737
		// (get) Token: 0x0600D968 RID: 55656 RVA: 0x00508ECB File Offset: 0x005070CB
		protected virtual SimpleCurve ComplexSizeOverPointsCurve
		{
			get
			{
				return new SimpleCurve
				{
					{
						new CurvePoint(0f, 30f),
						true
					},
					{
						new CurvePoint(10000f, 50f),
						true
					}
				};
			}
		}

		// Token: 0x1700260A RID: 9738
		// (get) Token: 0x0600D969 RID: 55657 RVA: 0x00508F00 File Offset: 0x00507100
		protected virtual SimpleCurve TerminalsOverRoomCountCurve
		{
			get
			{
				return new SimpleCurve
				{
					{
						new CurvePoint(0f, 1f),
						true
					},
					{
						new CurvePoint(10f, 4f),
						true
					},
					{
						new CurvePoint(20f, 6f),
						true
					},
					{
						new CurvePoint(50f, 10f),
						true
					}
				};
			}
		}

		// Token: 0x1700260B RID: 9739
		// (get) Token: 0x0600D96A RID: 55658 RVA: 0x00508F6A File Offset: 0x0050716A
		protected virtual LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.AncientComplex;
			}
		}

		// Token: 0x1700260C RID: 9740
		// (get) Token: 0x0600D96B RID: 55659 RVA: 0x00508F71 File Offset: 0x00507171
		protected virtual SitePartDef SitePartDef
		{
			get
			{
				return SitePartDefOf.AncientComplex;
			}
		}

		// Token: 0x1700260D RID: 9741
		// (get) Token: 0x0600D96C RID: 55660 RVA: 0x00508F78 File Offset: 0x00507178
		protected static List<LandmarkDef> AllowedLandmarks
		{
			get
			{
				if (ModsConfig.OdysseyActive && QuestNode_Root_AncientComplex.allowedLandmarksCached == null)
				{
					QuestNode_Root_AncientComplex.allowedLandmarksCached = new List<LandmarkDef>
					{
						LandmarkDefOf.Oasis,
						LandmarkDefOf.Lake,
						LandmarkDefOf.LakeWithIsland,
						LandmarkDefOf.LakeWithIslands,
						LandmarkDefOf.Pond,
						LandmarkDefOf.DryLake,
						LandmarkDefOf.ToxicLake,
						LandmarkDefOf.Wetland,
						LandmarkDefOf.HotSprings,
						LandmarkDefOf.CoastalIsland,
						LandmarkDefOf.Peninsula,
						LandmarkDefOf.Valley,
						LandmarkDefOf.Cavern,
						LandmarkDefOf.Chasm,
						LandmarkDefOf.IceDunes,
						LandmarkDefOf.Cliffs,
						LandmarkDefOf.Hollow,
						LandmarkDefOf.TerraformingScar,
						LandmarkDefOf.LavaFlow,
						LandmarkDefOf.Dunes,
						LandmarkDefOf.AncientSmokeVent,
						LandmarkDefOf.AncientToxVent
					};
				}
				return QuestNode_Root_AncientComplex.allowedLandmarksCached;
			}
		}

		// Token: 0x0600D96D RID: 55661
		public abstract LayoutStructureSketch QuestSetupComplex(Quest quest, float points);

		// Token: 0x0600D96E RID: 55662 RVA: 0x0050909C File Offset: 0x0050729C
		protected virtual LayoutStructureSketch GenerateStructureSketch(float points, bool generateTerminals = true)
		{
			int num = (int)this.ComplexSizeOverPointsCurve.Evaluate(points);
			StructureGenParams parms = new StructureGenParams
			{
				size = new IntVec2(num, num)
			};
			LayoutStructureSketch layoutStructureSketch = this.LayoutDef.Worker.GenerateStructureSketch(parms);
			if (generateTerminals)
			{
				int num2 = Mathf.FloorToInt(this.TerminalsOverRoomCountCurve.Evaluate((float)layoutStructureSketch.structureLayout.Rooms.Count));
				for (int i = 0; i < num2; i++)
				{
					layoutStructureSketch.thingsToSpawn.Add(ThingMaker.MakeThing(ThingDefOf.AncientTerminal, null));
				}
			}
			return layoutStructureSketch;
		}

		// Token: 0x04008AF6 RID: 35574
		private static List<LandmarkDef> allowedLandmarksCached;

		// Token: 0x04008AF7 RID: 35575
		protected static readonly SimpleCurve ThreatPointsOverPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(35f, 38.5f),
				true
			},
			{
				new CurvePoint(400f, 165f),
				true
			},
			{
				new CurvePoint(10000f, 4125f),
				true
			}
		};
	}
}
