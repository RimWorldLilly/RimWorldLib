using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001ABA RID: 6842
	[StaticConstructorOnStartup]
	public class Building_HoldingPlatform : Building, IThingHolderWithDrawnPawn, IThingHolder, IRoofCollapseAlert, ISearchableContents
	{
		// Token: 0x17001F26 RID: 7974
		// (get) Token: 0x0600ADCE RID: 44494 RVA: 0x003D4227 File Offset: 0x003D2427
		public float HeldPawnDrawPos_Y
		{
			get
			{
				return this.DrawPos.y + 0.03658537f;
			}
		}

		// Token: 0x17001F27 RID: 7975
		// (get) Token: 0x0600ADCF RID: 44495 RVA: 0x003D423C File Offset: 0x003D243C
		public float HeldPawnBodyAngle
		{
			get
			{
				return base.Rotation.AsAngle;
			}
		}

		// Token: 0x17001F28 RID: 7976
		// (get) Token: 0x0600ADD0 RID: 44496 RVA: 0x003D4257 File Offset: 0x003D2457
		public PawnPosture HeldPawnPosture
		{
			get
			{
				return PawnPosture.LayingOnGroundFaceUp;
			}
		}

		// Token: 0x17001F29 RID: 7977
		// (get) Token: 0x0600ADD1 RID: 44497 RVA: 0x003D425A File Offset: 0x003D245A
		public Rot4 HeldPawnRotation
		{
			get
			{
				return base.Rotation;
			}
		}

		// Token: 0x17001F2A RID: 7978
		// (get) Token: 0x0600ADD2 RID: 44498 RVA: 0x003D4262 File Offset: 0x003D2462
		public Vector3 PawnDrawOffset
		{
			get
			{
				return new Vector3(0f, 0f, 0.15f);
			}
		}

		// Token: 0x17001F2B RID: 7979
		// (get) Token: 0x0600ADD3 RID: 44499 RVA: 0x003D4278 File Offset: 0x003D2478
		public Pawn HeldPawn
		{
			get
			{
				return this.innerContainer.FirstOrDefault((Thing x) => x is Pawn) as Pawn;
			}
		}

		// Token: 0x17001F2C RID: 7980
		// (get) Token: 0x0600ADD4 RID: 44500 RVA: 0x003D42A9 File Offset: 0x003D24A9
		public bool Occupied
		{
			get
			{
				return this.HeldPawn != null;
			}
		}

		// Token: 0x17001F2D RID: 7981
		// (get) Token: 0x0600ADD5 RID: 44501 RVA: 0x003D42B4 File Offset: 0x003D24B4
		public float AnimationAlpha
		{
			get
			{
				return Mathf.Clamp01((float)(Find.TickManager.TicksGame - this.heldPawnStartTick) / 20f);
			}
		}

		// Token: 0x17001F2E RID: 7982
		// (get) Token: 0x0600ADD6 RID: 44502 RVA: 0x003D42D4 File Offset: 0x003D24D4
		private CompAffectedByFacilities FacilitiesComp
		{
			get
			{
				CompAffectedByFacilities result;
				if ((result = this.facilitiesComp) == null)
				{
					result = (this.facilitiesComp = base.GetComp<CompAffectedByFacilities>());
				}
				return result;
			}
		}

		// Token: 0x17001F2F RID: 7983
		// (get) Token: 0x0600ADD7 RID: 44503 RVA: 0x003D42FC File Offset: 0x003D24FC
		private CompAttachPoints AttachPointsComp
		{
			get
			{
				CompAttachPoints result;
				if ((result = this.attachPointsComp) == null)
				{
					result = (this.attachPointsComp = base.GetComp<CompAttachPoints>());
				}
				return result;
			}
		}

		// Token: 0x17001F30 RID: 7984
		// (get) Token: 0x0600ADD8 RID: 44504 RVA: 0x003D4322 File Offset: 0x003D2522
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x17001F31 RID: 7985
		// (get) Token: 0x0600ADD9 RID: 44505 RVA: 0x003D432C File Offset: 0x003D252C
		private AttachPointTracker TargetPawnAttachPoints
		{
			get
			{
				if (this.targetPoints != null && this.targetPoints.ThingId != this.HeldPawn.ThingID)
				{
					this.targetPoints = null;
				}
				bool flag = this.targetPoints == null;
				AttachPointTracker attachPointTracker;
				if ((attachPointTracker = this.targetPoints) == null)
				{
					CompAttachPoints compAttachPoints = this.HeldPawn.TryGetComp<CompAttachPoints>();
					attachPointTracker = ((compAttachPoints != null) ? compAttachPoints.points : null);
				}
				this.targetPoints = attachPointTracker;
				if (flag)
				{
					foreach (HediffComp_AttachPoints hediffComp_AttachPoints in this.HeldPawn.health.hediffSet.GetHediffComps<HediffComp_AttachPoints>())
					{
						if (hediffComp_AttachPoints.Points != null)
						{
							if (this.targetPoints == null)
							{
								this.targetPoints = hediffComp_AttachPoints.Points;
							}
							else
							{
								this.targetPoints.Add(hediffComp_AttachPoints.Points, true);
							}
						}
					}
				}
				return this.targetPoints;
			}
		}

		// Token: 0x17001F32 RID: 7986
		// (get) Token: 0x0600ADDA RID: 44506 RVA: 0x003D4418 File Offset: 0x003D2618
		public bool HasAttachedElectroharvester
		{
			get
			{
				foreach (Thing thing in this.FacilitiesComp.LinkedFacilitiesListForReading)
				{
					CompPowerPlantElectroharvester compPowerPlantElectroharvester = thing.TryGetComp<CompPowerPlantElectroharvester>();
					if (compPowerPlantElectroharvester != null && compPowerPlantElectroharvester.PowerOn)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001F33 RID: 7987
		// (get) Token: 0x0600ADDB RID: 44507 RVA: 0x003D4480 File Offset: 0x003D2680
		public bool HasAttachedBioferriteHarvester
		{
			get
			{
				foreach (Thing thing in this.FacilitiesComp.LinkedFacilitiesListForReading)
				{
					Building_BioferriteHarvester building_BioferriteHarvester = thing as Building_BioferriteHarvester;
					if (building_BioferriteHarvester != null && building_BioferriteHarvester.Power.PowerOn)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001F34 RID: 7988
		// (get) Token: 0x0600ADDC RID: 44508 RVA: 0x003D44F0 File Offset: 0x003D26F0
		private Graphic ChainsUntetheredGraphic
		{
			get
			{
				if (this.chainsUntetheredGraphic == null)
				{
					this.chainsUntetheredGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.untetheredGraphicTexPath, ShaderDatabase.Cutout, this.def.graphicData.drawSize, Color.white);
				}
				return this.chainsUntetheredGraphic;
			}
		}

		// Token: 0x17001F35 RID: 7989
		// (get) Token: 0x0600ADDD RID: 44509 RVA: 0x003D4530 File Offset: 0x003D2730
		private CompProperties_EntityHolderPlatform PlatformProps
		{
			get
			{
				return base.GetComp<CompEntityHolderPlatform>().Props;
			}
		}

		// Token: 0x0600ADDE RID: 44510 RVA: 0x003D453D File Offset: 0x003D273D
		public Building_HoldingPlatform()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600ADDF RID: 44511 RVA: 0x003D455F File Offset: 0x003D275F
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Holding platform"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			Find.StudyManager.UpdateStudiableCache(this, base.Map);
			Find.Anomaly.hasBuiltHoldingPlatform = true;
		}

		// Token: 0x0600ADE0 RID: 44512 RVA: 0x003D4599 File Offset: 0x003D2799
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!base.BeingTransportedOnGravship)
			{
				this.EjectContents();
			}
			this.platformPoints = null;
			base.DeSpawn(mode);
		}

		// Token: 0x0600ADE1 RID: 44513 RVA: 0x003D45B7 File Offset: 0x003D27B7
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600ADE2 RID: 44514 RVA: 0x003D45C5 File Offset: 0x003D27C5
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600ADE3 RID: 44515 RVA: 0x003D45D0 File Offset: 0x003D27D0
		private bool TryGetFirstColonistDirection(out Vector2 direction)
		{
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(base.Position, base.Map, 4f, false))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.IsColonist)
				{
					direction = pawn.Position.ToVector2() - base.Position.ToVector2();
					return true;
				}
			}
			direction = Vector2.zero;
			return false;
		}

		// Token: 0x0600ADE4 RID: 44516 RVA: 0x003D4670 File Offset: 0x003D2870
		protected override void Tick()
		{
			base.Tick();
			if (this.Occupied && this.chains == null && this.AttachPointsComp != null)
			{
				this.chains = ((this.TargetPawnAttachPoints != null) ? this.BuildTargetPointMapping() : this.DefaultPointMapping);
			}
			if (!this.Occupied && this.chains != null)
			{
				this.chains = null;
			}
			if (this.Occupied && this.HasAttachedElectroharvester && Rand.MTBEventOccurs(2f, 60000f, 1f))
			{
				this.HeldPawn.TakeDamage(new DamageInfo(DamageDefOf.ElectricalBurn, Building_HoldingPlatform.Damage.RandomInRange, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			if (this.Occupied && Rand.MTBEventOccurs(100f, 1f, 1f))
			{
				this.UpdateAnimation();
			}
			if (this.debugEscapeTick > 0 && Find.TickManager.TicksGame == this.debugEscapeTick && this.HeldPawn != null)
			{
				CompHoldingPlatformTarget compHoldingPlatformTarget = this.HeldPawn.TryGetComp<CompHoldingPlatformTarget>();
				if (compHoldingPlatformTarget != null)
				{
					compHoldingPlatformTarget.Escape(false);
				}
			}
			if (this.heldPawnStartTick == -1 && this.HeldPawn != null)
			{
				this.heldPawnStartTick = Find.TickManager.TicksGame;
				return;
			}
			if (this.HeldPawn == null)
			{
				this.heldPawnStartTick = -1;
			}
		}

		// Token: 0x0600ADE5 RID: 44517 RVA: 0x003D47BC File Offset: 0x003D29BC
		private void UpdateAnimation()
		{
			CompHoldingPlatformTarget compHoldingPlatformTarget;
			if (this.HeldPawn.TryGetComp(out compHoldingPlatformTarget) && (!compHoldingPlatformTarget.Props.hasAnimation || this.HeldPawn.health.Downed))
			{
				this.HeldPawn.Drawer.renderer.SetAnimation(null);
				return;
			}
			SoundDef soundDef = this.PlatformProps.entityLungeSoundLow;
			AnimationDef animationDef = AnimationDefOf.HoldingPlatformWiggleLight;
			Vector2 vector;
			if (this.TryGetFirstColonistDirection(out vector))
			{
				if (this.TargetPawnAttachPoints != null && Rand.Chance(0.25f))
				{
					Vector2 lhs = vector.normalized.Cardinalize();
					if (lhs == Vector2.up)
					{
						animationDef = AnimationDefOf.HoldingPlatformLungeUp;
					}
					if (lhs == Vector2.right)
					{
						animationDef = AnimationDefOf.HoldingPlatformLungeRight;
					}
					if (lhs == Vector2.left)
					{
						animationDef = AnimationDefOf.HoldingPlatformLungeLeft;
					}
					if (lhs == Vector2.down)
					{
						animationDef = AnimationDefOf.HoldingPlatformLungeDown;
					}
					soundDef = this.PlatformProps.entityLungeSoundHi;
				}
				else
				{
					animationDef = AnimationDefOf.HoldingPlatformWiggleIntense;
				}
			}
			if (this.HeldPawn.Drawer.renderer.CurAnimation != animationDef)
			{
				if (soundDef != null)
				{
					soundDef.PlayOneShot(this);
				}
				this.HeldPawn.Drawer.renderer.SetAnimation(animationDef);
			}
		}

		// Token: 0x17001F36 RID: 7990
		// (get) Token: 0x0600ADE6 RID: 44518 RVA: 0x003D48E8 File Offset: 0x003D2AE8
		public PawnDrawParms HeldPawnDrawParms
		{
			get
			{
				return new PawnDrawParms
				{
					pawn = this.HeldPawn,
					facing = this.HeldPawn.Rotation,
					rotDrawMode = RotDrawMode.Fresh,
					posture = this.HeldPawn.GetPosture(),
					flags = (PawnRenderFlags.Headgear | PawnRenderFlags.Clothes),
					tint = Color.white
				};
			}
		}

		// Token: 0x0600ADE7 RID: 44519 RVA: 0x003D494C File Offset: 0x003D2B4C
		public List<Building_HoldingPlatform.Chain> BuildTargetPointMapping()
		{
			if (this.chains == null)
			{
				this.chains = new List<Building_HoldingPlatform.Chain>();
			}
			else
			{
				this.chains.Clear();
			}
			Vector3 a;
			Vector3 vector;
			Quaternion rotation;
			Vector3 vector2;
			this.HeldPawn.Drawer.renderer.renderTree.GetRootTPRS(this.HeldPawnDrawParms, out a, out vector, out rotation, out vector2);
			Vector3 a2 = this.DrawPos + this.PawnDrawOffset;
			Dictionary<AttachPointType, Vector3> dictionary = new Dictionary<AttachPointType, Vector3>();
			int num = 5;
			int num2 = 8;
			foreach (AttachPointType attachPointType in this.TargetPawnAttachPoints.PointTypes(num, num2))
			{
				Vector3 value = a2 + rotation * (a + this.TargetPawnAttachPoints.GetRotatedOffset(attachPointType, base.Rotation));
				dictionary.Add(attachPointType, value);
			}
			for (int i = num; i <= num2; i++)
			{
				Vector3 vector3 = this.GetPlatformPoints()[(AttachPointType)i];
				Vector3 vector4 = dictionary[(AttachPointType)i];
				Vector3 vector5 = Vector3.Lerp(vector3, vector4, this.AnimationAlpha);
				float x = Vector3.Distance(vector5, vector3);
				Vector2 vector6 = new Vector2(x, 1f);
				this.chains.Add(new Building_HoldingPlatform.Chain
				{
					from = vector3,
					to = vector5,
					graphic = (GraphicDatabase.Get<Graphic_Tiling>(this.PlatformProps.tilingChainTexPath, ShaderTypeDefOf.CutoutTiling.Shader, vector6, Color.white) as Graphic_Tiling).WithTiling(vector6),
					baseFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.baseChainFastenerTexPath, ShaderTypeDefOf.CutoutTiling.Shader, Vector2.one, Color.white),
					targetFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.targetChainFastenerTexPath, ShaderTypeDefOf.CutoutTiling.Shader, Vector2.one, Color.white),
					rotation = (vector4.WithY(0f) - vector3.WithY(0f)).normalized.ToAngleFlat()
				});
			}
			return this.chains;
		}

		// Token: 0x0600ADE8 RID: 44520 RVA: 0x003D4B84 File Offset: 0x003D2D84
		private Dictionary<AttachPointType, Vector3> GetPlatformPoints()
		{
			if (this.platformPoints == null)
			{
				this.platformPoints = new Dictionary<AttachPointType, Vector3>();
				int min = 5;
				int max = 8;
				foreach (AttachPointType attachPointType in this.AttachPointsComp.points.PointTypes(min, max))
				{
					this.platformPoints.Add(attachPointType, this.AttachPointsComp.points.GetWorldPos(attachPointType));
				}
			}
			return this.platformPoints;
		}

		// Token: 0x17001F37 RID: 7991
		// (get) Token: 0x0600ADE9 RID: 44521 RVA: 0x003D4C10 File Offset: 0x003D2E10
		public List<Building_HoldingPlatform.Chain> DefaultPointMapping
		{
			get
			{
				if (this.defaultPointMapping == null)
				{
					this.defaultPointMapping = new List<Building_HoldingPlatform.Chain>();
					Vector3 worldPos = this.AttachPointsComp.points.GetWorldPos(AttachPointType.PlatformRestraint0);
					Vector3 worldPos2 = this.AttachPointsComp.points.GetWorldPos(AttachPointType.PlatformRestraint1);
					Vector3 worldPos3 = this.AttachPointsComp.points.GetWorldPos(AttachPointType.PlatformRestraint2);
					Vector3 worldPos4 = this.AttachPointsComp.points.GetWorldPos(AttachPointType.PlatformRestraint3);
					Vector2 vector = new Vector2(Vector3.Distance(worldPos, worldPos3), 1f);
					this.defaultPointMapping.Add(new Building_HoldingPlatform.Chain
					{
						from = worldPos,
						to = worldPos3,
						graphic = (GraphicDatabase.Get<Graphic_Tiling>(this.PlatformProps.tilingChainTexPath, ShaderTypeDefOf.Cutout.Shader, vector, Color.white) as Graphic_Tiling).WithTiling(vector),
						baseFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.baseChainFastenerTexPath, ShaderTypeDefOf.Cutout.Shader, Vector2.one, Color.white),
						targetFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.targetChainFastenerTexPath, ShaderTypeDefOf.Cutout.Shader, Vector2.one, Color.white),
						rotation = (worldPos3.WithY(0f) - worldPos.WithY(0f)).normalized.ToAngleFlat()
					});
					vector = new Vector2(Vector3.Distance(worldPos2, worldPos4), 1f);
					this.defaultPointMapping.Add(new Building_HoldingPlatform.Chain
					{
						from = worldPos2,
						to = worldPos4,
						graphic = (GraphicDatabase.Get<Graphic_Tiling>(this.PlatformProps.tilingChainTexPath, ShaderTypeDefOf.Cutout.Shader, vector, Color.white) as Graphic_Tiling).WithTiling(vector),
						baseFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.baseChainFastenerTexPath, ShaderTypeDefOf.Cutout.Shader, Vector2.one, Color.white),
						targetFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.targetChainFastenerTexPath, ShaderTypeDefOf.Cutout.Shader, Vector2.one, Color.white),
						rotation = (worldPos4.WithY(0f) - worldPos2.WithY(0f)).normalized.ToAngleFlat()
					});
				}
				return this.defaultPointMapping;
			}
		}

		// Token: 0x0600ADEA RID: 44522 RVA: 0x003D4E68 File Offset: 0x003D3068
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
			Pawn heldPawn = this.HeldPawn;
			if (heldPawn != null)
			{
				Rot4 value = Rot4.South;
				if (heldPawn.IsShambler && heldPawn.RaceProps.Animal)
				{
					value = Rot4.East;
				}
				heldPawn.Drawer.renderer.DynamicDrawPhaseAt(phase, this.DrawPos + this.PawnDrawOffset, new Rot4?(value), true);
			}
		}

		// Token: 0x0600ADEB RID: 44523 RVA: 0x003D4ED4 File Offset: 0x003D30D4
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (this.HeldPawn != null)
			{
				this.DrawChains();
				return;
			}
			this.ChainsUntetheredGraphic.Draw(drawLoc + Vector3.up * 0.05f, base.Rotation, this, 0f);
		}

		// Token: 0x0600ADEC RID: 44524 RVA: 0x003D4F24 File Offset: 0x003D3124
		private void DrawChains()
		{
			if (this.chains != null)
			{
				this.chains = ((this.TargetPawnAttachPoints != null) ? this.BuildTargetPointMapping() : this.DefaultPointMapping);
				Vector3 vector = Vector3.up * 0.13658537f;
				foreach (Building_HoldingPlatform.Chain chain in this.chains)
				{
					Vector3 v = (chain.from + chain.to) / 2f;
					chain.graphic.Draw(v.WithY(this.DrawPos.y) + vector, base.Rotation, this, chain.rotation + 180f);
					chain.targetFastenerGraphic.Draw(chain.to + 2f * vector, base.Rotation, this, chain.rotation + 90f);
					chain.baseFastenerGraphic.Draw(chain.from + 2f * vector, base.Rotation, this, chain.rotation + 90f);
				}
			}
		}

		// Token: 0x0600ADED RID: 44525 RVA: 0x003D5068 File Offset: 0x003D3268
		public void EjectContents()
		{
			this.defaultPointMapping = null;
			this.chains = null;
			Pawn heldPawn = this.HeldPawn;
			if (heldPawn != null)
			{
				heldPawn.Drawer.renderer.SetAnimation(null);
			}
			Pawn heldPawn2 = this.HeldPawn;
			if (heldPawn2 != null)
			{
				CompHoldingPlatformTarget comp = heldPawn2.GetComp<CompHoldingPlatformTarget>();
				if (comp != null)
				{
					comp.Notify_ReleasedFromPlatform();
				}
			}
			this.innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
		}

		// Token: 0x0600ADEE RID: 44526 RVA: 0x003D50D7 File Offset: 0x003D32D7
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Building_HoldingPlatform.<GetFloatMenuOptions>d__67 <GetFloatMenuOptions>d__ = new Building_HoldingPlatform.<GetFloatMenuOptions>d__67(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600ADEF RID: 44527 RVA: 0x003D50F0 File Offset: 0x003D32F0
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			Pawn heldPawn = this.HeldPawn;
			if (heldPawn != null)
			{
				TaggedString taggedString = "HoldingThing".Translate() + ": " + heldPawn.NameShortColored.CapitalizeFirst();
				bool flag = this.SafelyContains(heldPawn);
				if (!flag)
				{
					taggedString += " (" + "HoldingPlatformRequiresStrength".Translate(StatDefOf.MinimumContainmentStrength.Worker.ValueToString(heldPawn.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1), false, ToStringNumberSense.Absolute)) + ")";
				}
				text += taggedString.Colorize(flag ? Color.white : ColorLibrary.RedReadable);
			}
			else
			{
				text += "HoldingThing".Translate() + ": " + "Nothing".Translate().CapitalizeFirst();
			}
			if (heldPawn != null && heldPawn.def.IsStudiable)
			{
				string inspectStringExtraFor = CompStudiable.GetInspectStringExtraFor(heldPawn);
				if (!inspectStringExtraFor.NullOrEmpty())
				{
					text = text + "\n" + inspectStringExtraFor;
				}
			}
			CompProducesBioferrite compProducesBioferrite;
			if (heldPawn != null && heldPawn.TryGetComp(out compProducesBioferrite))
			{
				string text2 = compProducesBioferrite.CompInspectStringExtra();
				if (!text2.NullOrEmpty())
				{
					text = text + "\n" + text2;
				}
			}
			return text;
		}

		// Token: 0x0600ADF0 RID: 44528 RVA: 0x003D5256 File Offset: 0x003D3456
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			Building_HoldingPlatform.<GetInspectTabs>d__69 <GetInspectTabs>d__ = new Building_HoldingPlatform.<GetInspectTabs>d__69(-2);
			<GetInspectTabs>d__.<>4__this = this;
			return <GetInspectTabs>d__;
		}

		// Token: 0x0600ADF1 RID: 44529 RVA: 0x003D5268 File Offset: 0x003D3468
		public void Notify_PawnDied(Pawn pawn, DamageInfo? dinfo)
		{
			if (pawn == this.HeldPawn)
			{
				this.innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
				if (dinfo == null || !dinfo.Value.Def.execution)
				{
					Messages.Message("EntityDiedOnHoldingPlatform".Translate(pawn), pawn, MessageTypeDefOf.NegativeEvent, true);
				}
			}
		}

		// Token: 0x0600ADF2 RID: 44530 RVA: 0x003D52DF File Offset: 0x003D34DF
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_HoldingPlatform.<GetGizmos>d__71 <GetGizmos>d__ = new Building_HoldingPlatform.<GetGizmos>d__71(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600ADF3 RID: 44531 RVA: 0x003D52F0 File Offset: 0x003D34F0
		public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
		{
			if (!this.Occupied)
			{
				return RoofCollapseResponse.None;
			}
			IRoofCollapseAlert roofCollapseAlert = this.HeldPawn as IRoofCollapseAlert;
			if (roofCollapseAlert != null)
			{
				roofCollapseAlert.Notify_OnBeforeRoofCollapse();
			}
			foreach (IRoofCollapseAlert roofCollapseAlert2 in this.HeldPawn.GetComps<IRoofCollapseAlert>())
			{
				roofCollapseAlert2.Notify_OnBeforeRoofCollapse();
			}
			return RoofCollapseResponse.None;
		}

		// Token: 0x0600ADF4 RID: 44532 RVA: 0x003D5364 File Offset: 0x003D3564
		public override void Notify_DefsHotReloaded()
		{
			base.Notify_DefsHotReloaded();
			this.chains = null;
		}

		// Token: 0x0600ADF5 RID: 44533 RVA: 0x003D5374 File Offset: 0x003D3574
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastDamaged, "lastDamaged", 0, false);
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.heldPawnStartTick, "heldPawnStartTick", 0, false);
		}

		// Token: 0x040064B2 RID: 25778
		public ThingOwner innerContainer;

		// Token: 0x040064B3 RID: 25779
		private int lastDamaged;

		// Token: 0x040064B4 RID: 25780
		private Graphic chainsUntetheredGraphic;

		// Token: 0x040064B5 RID: 25781
		private List<Building_HoldingPlatform.Chain> chains;

		// Token: 0x040064B6 RID: 25782
		private CompAffectedByFacilities facilitiesComp;

		// Token: 0x040064B7 RID: 25783
		private CompAttachPoints attachPointsComp;

		// Token: 0x040064B8 RID: 25784
		private AttachPointTracker targetPoints;

		// Token: 0x040064B9 RID: 25785
		private List<Building_HoldingPlatform.Chain> defaultPointMapping;

		// Token: 0x040064BA RID: 25786
		[Unsaved(false)]
		private int debugEscapeTick = -1;

		// Token: 0x040064BB RID: 25787
		private int heldPawnStartTick = -1;

		// Token: 0x040064BC RID: 25788
		private const float ChainsUntetheredYOffset = 0.05f;

		// Token: 0x040064BD RID: 25789
		private const float ChainsTetheredYOffset = 0.13658537f;

		// Token: 0x040064BE RID: 25790
		private const float LurchMTBTicks = 100f;

		// Token: 0x040064BF RID: 25791
		private const float DamageMTBDays = 2f;

		// Token: 0x040064C0 RID: 25792
		private static readonly FloatRange Damage = new FloatRange(1f, 3f);

		// Token: 0x040064C1 RID: 25793
		private const float LungeAnimationChance = 0.25f;

		// Token: 0x040064C2 RID: 25794
		private Dictionary<AttachPointType, Vector3> platformPoints;

		// Token: 0x020037C8 RID: 14280
		public struct Chain
		{
			// Token: 0x0400DED4 RID: 57044
			public Vector3 from;

			// Token: 0x0400DED5 RID: 57045
			public Vector3 to;

			// Token: 0x0400DED6 RID: 57046
			public Graphic graphic;

			// Token: 0x0400DED7 RID: 57047
			public Graphic baseFastenerGraphic;

			// Token: 0x0400DED8 RID: 57048
			public Graphic targetFastenerGraphic;

			// Token: 0x0400DED9 RID: 57049
			public float rotation;
		}
	}
}
