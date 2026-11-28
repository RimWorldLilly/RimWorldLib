using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200038F RID: 911
	public class PawnRenderer
	{
		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001BA6 RID: 7078 RVA: 0x000AA900 File Offset: 0x000A8B00
		public Graphic BodyGraphic
		{
			get
			{
				return this.renderTree.BodyGraphic;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x000AA90D File Offset: 0x000A8B0D
		public Graphic HeadGraphic
		{
			get
			{
				return this.renderTree.HeadGraphic;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001BA8 RID: 7080 RVA: 0x000AA91C File Offset: 0x000A8B1C
		public RotDrawMode CurRotDrawMode
		{
			get
			{
				if (this.pawn.IsMutant && this.pawn.mutant.Def.useCorpseGraphics)
				{
					return RottableUtility.GetRotDrawMode(this.pawn.mutant.rotStage);
				}
				if (this.pawn.Dead && this.pawn.Corpse != null)
				{
					return this.pawn.Corpse.CurRotDrawMode;
				}
				return RotDrawMode.Fresh;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001BA9 RID: 7081 RVA: 0x000AA98F File Offset: 0x000A8B8F
		public PawnWoundDrawer WoundOverlays
		{
			get
			{
				return this.woundOverlays;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001BAA RID: 7082 RVA: 0x000AA997 File Offset: 0x000A8B97
		public PawnFirefoamDrawer FirefoamOverlays
		{
			get
			{
				return this.firefoamOverlays;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001BAB RID: 7083 RVA: 0x000AA99F File Offset: 0x000A8B9F
		public PawnShamblerScarDrawer ShamblerScarDrawer
		{
			get
			{
				return this.shamblerScarDrawer;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001BAC RID: 7084 RVA: 0x000AA9A7 File Offset: 0x000A8BA7
		public PawnScariaSoreDrawer ScariaSoreDrawer
		{
			get
			{
				return this.scariaSoreDrawer;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001BAD RID: 7085 RVA: 0x000AA9AF File Offset: 0x000A8BAF
		public PawnHeadOverlays StatusOverlays
		{
			get
			{
				return this.statusOverlays;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001BAE RID: 7086 RVA: 0x000AA9B7 File Offset: 0x000A8BB7
		public AnimationDef CurAnimation
		{
			get
			{
				return this.renderTree.currentAnimation;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001BAF RID: 7087 RVA: 0x000AA9C4 File Offset: 0x000A8BC4
		public bool HasAnimation
		{
			get
			{
				return this.renderTree.currentAnimation != null;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001BB0 RID: 7088 RVA: 0x000AA9D4 File Offset: 0x000A8BD4
		public Graphic SilhouetteGraphic
		{
			get
			{
				return this.silhouetteGraphic;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001BB1 RID: 7089 RVA: 0x000AA9DC File Offset: 0x000A8BDC
		public Vector3 SilhouettePos
		{
			get
			{
				return this.silhouettePos;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001BB2 RID: 7090 RVA: 0x000AA9E4 File Offset: 0x000A8BE4
		public Material FlightShadowMaterial
		{
			get
			{
				return MaterialPool.MatFrom("Things/Skyfaller/SkyfallerShadowCircle", ShaderDatabase.Transparent, new Color(1f, 1f, 1f, 0.5f));
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001BB3 RID: 7091 RVA: 0x000AAA10 File Offset: 0x000A8C10
		public Color? StatueColor
		{
			get
			{
				if (this.statueStuff == null)
				{
					return null;
				}
				Color? result = this.statuePaintColor;
				if (result != null)
				{
					return result;
				}
				ThingDef thingDef = this.statueStuff;
				if (thingDef == null)
				{
					return null;
				}
				StuffProperties stuffProps = thingDef.stuffProps;
				if (stuffProps == null)
				{
					return null;
				}
				return new Color?(stuffProps.color);
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x000AAA74 File Offset: 0x000A8C74
		private PawnRenderFlags DefaultRenderFlagsNow
		{
			get
			{
				PawnRenderFlags pawnRenderFlags;
				this.PawnNeedsHediffMaterial(out pawnRenderFlags);
				if (!this.pawn.health.hediffSet.HasHead)
				{
					pawnRenderFlags |= PawnRenderFlags.HeadStump;
				}
				if (this.statueStuff != null)
				{
					pawnRenderFlags |= PawnRenderFlags.Statue;
				}
				return pawnRenderFlags;
			}
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x000AAAB8 File Offset: 0x000A8CB8
		public PawnRenderer(Pawn pawn)
		{
			this.pawn = pawn;
			this.wiggler = new PawnDownedWiggler(pawn);
			this.statusOverlays = new PawnHeadOverlays(pawn);
			this.woundOverlays = new PawnWoundDrawer(pawn);
			this.firefoamOverlays = new PawnFirefoamDrawer(pawn);
			this.shamblerScarDrawer = new PawnShamblerScarDrawer(pawn);
			this.scariaSoreDrawer = new PawnScariaSoreDrawer(pawn);
			this.effecters = new PawnStatusEffecters(pawn);
			this.renderTree = new PawnRenderTree(pawn);
			this.flasher = new DamageFlasher(pawn);
			if (pawn.RaceProps.startingAnimation != null)
			{
				this.SetAnimation(pawn.RaceProps.startingAnimation);
			}
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x000AAB67 File Offset: 0x000A8D67
		public void SetStatue(ThingDef stuff)
		{
			this.statueStuff = stuff;
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x000AAB70 File Offset: 0x000A8D70
		public void SetStatuePaintColor(Color? color)
		{
			this.statuePaintColor = color;
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x000AAB7C File Offset: 0x000A8D7C
		private Mesh GetBlitMeshUpdatedFrame(PawnTextureAtlasFrameSet frameSet, Rot4 rotation, PawnDrawMode drawMode)
		{
			int index = frameSet.GetIndex(rotation, drawMode);
			if (frameSet.isDirty[index])
			{
				Find.PawnCacheCamera.rect = frameSet.uvRects[index];
				Find.PawnCacheRenderer.RenderPawn(this.pawn, frameSet.atlas, Vector3.zero, 1f, 0f, rotation, true, true, true, false, default(Vector3), null, null, false);
				Find.PawnCacheCamera.rect = new Rect(0f, 0f, 1f, 1f);
				frameSet.isDirty[index] = false;
			}
			return frameSet.meshes[index];
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x000AAC28 File Offset: 0x000A8E28
		private void DrawShadowInternal(Vector3 drawLoc)
		{
			if (this.pawn.Flying)
			{
				Vector3 vector = this.pawn.DrawPos;
				vector.y = AltitudeLayer.Filth.AltitudeFor();
				vector -= new Vector3(0f, 0f, this.pawn.flight.PositionOffsetFactor);
				Matrix4x4 matrix = Matrix4x4.TRS(vector, Quaternion.identity, Vector3.one * 0.75f);
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.FlightShadowMaterial, 0);
				return;
			}
			if (this.pawn.Swimming || this.pawn.DrawNonHumanlikeSwimmingGraphic)
			{
				return;
			}
			if (this.pawn.def.race.specialShadowData != null)
			{
				if (this.shadowGraphic == null)
				{
					this.shadowGraphic = new Graphic_Shadow(this.pawn.def.race.specialShadowData);
				}
				this.shadowGraphic.Draw(drawLoc, Rot4.North, this.pawn, 0f);
			}
			Graphic bodyGraphic = this.BodyGraphic;
			if (bodyGraphic == null)
			{
				return;
			}
			Graphic_Shadow graphic_Shadow = bodyGraphic.ShadowGraphic;
			if (graphic_Shadow == null)
			{
				return;
			}
			graphic_Shadow.Draw(drawLoc, Rot4.North, this.pawn, 0f);
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x000AAD50 File Offset: 0x000A8F50
		public void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, Rot4? rotOverride = null, bool neverAimWeapon = false)
		{
			if (phase == DrawPhase.EnsureInitialized)
			{
				this.EnsureGraphicsInitialized();
				return;
			}
			if (phase == DrawPhase.ParallelPreDraw)
			{
				this.ParallelPreRenderPawnAt(drawLoc, rotOverride, neverAimWeapon);
				return;
			}
			if (phase == DrawPhase.Draw)
			{
				this.RenderPawnAt(drawLoc, rotOverride, neverAimWeapon);
			}
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x000AAD7C File Offset: 0x000A8F7C
		public void RenderPawnAt(Vector3 drawLoc, Rot4? rotOverride = null, bool neverAimWeapon = false)
		{
			if (!this.results.valid)
			{
				this.EnsureGraphicsInitialized();
				this.ParallelPreRenderPawnAt(drawLoc, rotOverride, neverAimWeapon);
			}
			if (!this.results.draw)
			{
				return;
			}
			if (this.results.useCached)
			{
				PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet;
				bool flag;
				if (GlobalTextureAtlasManager.TryGetPawnFrameSet(this.pawn, out pawnTextureAtlasFrameSet, out flag, true))
				{
					using (new ProfilerBlock("Draw Cached Mesh"))
					{
						Rot4 facing = this.results.parms.facing;
						Vector3 bodyPos = this.results.bodyPos;
						float bodyAngle = this.results.bodyAngle;
						PawnDrawMode drawMode = this.results.showBody ? PawnDrawMode.BodyAndHead : PawnDrawMode.HeadOnly;
						Material mat = this.OverrideMaterialIfNeeded(MaterialPool.MatFrom(new MaterialRequest(pawnTextureAtlasFrameSet.atlas, ShaderDatabase.Cutout)), PawnRenderFlags.None);
						GenDraw.DrawMeshNowOrLater(this.GetBlitMeshUpdatedFrame(pawnTextureAtlasFrameSet, facing, drawMode), bodyPos, Quaternion.AngleAxis(bodyAngle, Vector3.up), mat, false);
						Vector3 drawPos = bodyPos.WithYOffset(PawnRenderUtility.AltitudeForLayer((facing == Rot4.North) ? -10f : 90f));
						PawnRenderUtility.DrawEquipmentAndApparelExtras(this.pawn, drawPos, facing, this.results.parms.flags);
						goto IL_1B7;
					}
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to use a cached frame set for pawn ");
				defaultInterpolatedStringHandler.AppendFormatted<Name>(this.pawn.Name);
				defaultInterpolatedStringHandler.AppendLiteral(" but failed to get one.");
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), Gen.HashCombine<int>(this.pawn.GetHashCode(), 5938111));
			}
			else
			{
				using (new ProfilerBlock("Render Pawn Internal"))
				{
					this.RenderPawnInternal(this.results.parms);
				}
			}
			IL_1B7:
			if (this.results.posture == PawnPosture.Standing && !this.results.parms.flags.FlagSet(PawnRenderFlags.Invisible))
			{
				using (new ProfilerBlock("Draw Shadow Internal"))
				{
					this.DrawShadowInternal(drawLoc);
				}
			}
			if (this.pawn.Spawned && !this.pawn.Dead)
			{
				this.pawn.stances.StanceTrackerDraw();
				this.pawn.pather.PatherDraw();
				this.pawn.roping.RopingDraw();
			}
			Graphic graphic;
			if (this.pawn.RaceProps.Humanlike)
			{
				graphic = this.pawn.ageTracker.CurLifeStage.silhouetteGraphicData.Graphic;
			}
			else if (this.pawn.ageTracker.CurKindLifeStage.silhouetteGraphicData != null)
			{
				graphic = this.pawn.ageTracker.CurKindLifeStage.silhouetteGraphicData.Graphic;
			}
			else
			{
				graphic = this.BodyGraphic;
			}
			this.SetSilhouetteData(graphic, this.results.bodyPos);
			this.DrawDebug();
			this.results = default(PawnRenderer.PreRenderResults);
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x000AB088 File Offset: 0x000A9288
		private void SetSilhouetteData(Graphic graphic, Vector3 pos)
		{
			this.silhouetteGraphic = graphic;
			this.silhouettePos = pos;
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x000AB098 File Offset: 0x000A9298
		public void RenderShadowOnlyAt(Vector3 drawLoc)
		{
			if (this.pawn.GetPosture() == PawnPosture.Standing && !this.DefaultRenderFlagsNow.FlagSet(PawnRenderFlags.Invisible))
			{
				this.DrawShadowInternal(drawLoc);
			}
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x000AB0BC File Offset: 0x000A92BC
		public void EnsureGraphicsInitialized()
		{
			this.renderTree.EnsureInitialized(this.DefaultRenderFlagsNow);
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000AB0CF File Offset: 0x000A92CF
		private void ParallelPreRenderPawnAt(Vector3 drawLoc, Rot4? rotOverride = null, bool neverAimWeapon = false)
		{
			this.results = this.ParallelGetPreRenderResults(drawLoc, rotOverride, neverAimWeapon, false);
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x000AB0E4 File Offset: 0x000A92E4
		private PawnRenderer.PreRenderResults ParallelGetPreRenderResults(Vector3 drawLoc, Rot4? rotOverride = null, bool neverAimWeapon = false, bool disableCache = false)
		{
			PawnRenderer.PreRenderResults preRenderResults = new PawnRenderer.PreRenderResults
			{
				valid = true
			};
			if (this.pawn.IsHiddenFromPlayer())
			{
				return preRenderResults;
			}
			preRenderResults.draw = true;
			if (!this.renderTree.Resolved)
			{
				this.renderTree.SetDirty();
			}
			PawnRenderFlags pawnRenderFlags = this.DefaultRenderFlagsNow | PawnRenderFlags.Clothes | PawnRenderFlags.Headgear;
			if (neverAimWeapon)
			{
				pawnRenderFlags |= PawnRenderFlags.NeverAimWeapon;
			}
			bool crawling = this.pawn.Crawling;
			bool swimming = this.pawn.Swimming;
			if (swimming)
			{
				pawnRenderFlags &= ~(PawnRenderFlags.Headgear | PawnRenderFlags.Clothes | PawnRenderFlags.NeverAimWeapon);
				pawnRenderFlags |= PawnRenderFlags.NoBody;
			}
			RotDrawMode curRotDrawMode = this.CurRotDrawMode;
			PawnPosture posture = this.pawn.GetPosture();
			bool showBody;
			Vector3 bodyPos = this.GetBodyPos(drawLoc, posture, out showBody);
			float num = (posture == PawnPosture.Standing) ? 0f : this.BodyAngle(pawnRenderFlags);
			Rot4 bodyFacing = rotOverride ?? ((posture == PawnPosture.Standing || crawling) ? this.pawn.Rotation : this.LayingFacing());
			preRenderResults.posture = posture;
			preRenderResults.showBody = showBody;
			preRenderResults.bodyPos = bodyPos;
			preRenderResults.bodyAngle = num;
			bool useCached;
			if (!disableCache && this.pawn.RaceProps.Humanlike && Find.CameraDriver.ZoomRootSize > 18f && !pawnRenderFlags.FlagSet(PawnRenderFlags.Portrait) && !pawnRenderFlags.FlagSet(PawnRenderFlags.Statue) && curRotDrawMode != RotDrawMode.Dessicated && !crawling && !swimming && this.renderTree.currentAnimation == null)
			{
				Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
				if (((carryTracker != null) ? carryTracker.CarriedThing : null) == null && this.pawn.CarriedBy == null)
				{
					PawnRenderFlags pawnRenderFlags2;
					useCached = !this.PawnNeedsHediffMaterial(out pawnRenderFlags2);
					goto IL_1A0;
				}
			}
			useCached = false;
			IL_1A0:
			preRenderResults.useCached = useCached;
			preRenderResults.parms = this.GetDrawParms(bodyPos, num, bodyFacing, curRotDrawMode, pawnRenderFlags);
			if (preRenderResults.useCached)
			{
				return preRenderResults;
			}
			this.renderTree.ParallelPreDraw(preRenderResults.parms);
			return preRenderResults;
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x000AB2C8 File Offset: 0x000A94C8
		public void RenderCache(Rot4 rotation, float angle, Vector3 positionOffset, bool renderHead, bool portrait, bool renderHeadgear, bool renderClothes, IReadOnlyDictionary<Apparel, Color> overrideApparelColor = null, Color? overrideHairColor = null, bool stylingStation = false)
		{
			Vector3 zero = Vector3.zero;
			PawnRenderFlags pawnRenderFlags = this.DefaultRenderFlagsNow | PawnRenderFlags.Cache | PawnRenderFlags.DrawNow;
			if (portrait)
			{
				pawnRenderFlags |= PawnRenderFlags.Portrait;
			}
			if (!renderHead)
			{
				pawnRenderFlags |= PawnRenderFlags.HeadStump;
			}
			if (renderHeadgear)
			{
				pawnRenderFlags |= PawnRenderFlags.Headgear;
			}
			if (renderClothes)
			{
				pawnRenderFlags |= PawnRenderFlags.Clothes;
			}
			if (stylingStation)
			{
				pawnRenderFlags |= PawnRenderFlags.StylingStation;
			}
			this.tmpOriginalColors.Clear();
			try
			{
				if (overrideApparelColor != null)
				{
					foreach (KeyValuePair<Apparel, Color> keyValuePair in overrideApparelColor)
					{
						Apparel apparel;
						Color color;
						keyValuePair.Deconstruct(out apparel, out color);
						Apparel apparel2 = apparel;
						Color newColor = color;
						CompColorable compColorable = apparel2.TryGetComp<CompColorable>();
						if (compColorable != null)
						{
							this.tmpOriginalColors.Add(apparel2, new ValueTuple<Color, bool>(compColorable.Color, compColorable.Active));
							apparel2.SetColor(newColor, true);
						}
					}
				}
				Color hairColor = Color.white;
				if (this.pawn.story != null)
				{
					hairColor = this.pawn.story.HairColor;
					if (overrideHairColor != null)
					{
						this.pawn.story.HairColor = overrideHairColor.Value;
						this.pawn.Drawer.renderer.renderTree.SetDirty();
					}
				}
				PawnDrawParms drawParms = this.GetDrawParms(zero + positionOffset, angle, rotation, this.CurRotDrawMode, pawnRenderFlags);
				this.renderTree.EnsureInitialized(this.DefaultRenderFlagsNow);
				this.renderTree.ParallelPreDraw(drawParms);
				this.RenderPawnInternal(drawParms);
				foreach (KeyValuePair<Apparel, ValueTuple<Color, bool>> keyValuePair2 in this.tmpOriginalColors)
				{
					if (!keyValuePair2.Value.Item2)
					{
						keyValuePair2.Key.TryGetComp<CompColorable>().Disable();
					}
					else
					{
						keyValuePair2.Key.SetColor(keyValuePair2.Value.Item1, true);
					}
				}
				if (this.pawn.story != null && overrideHairColor != null)
				{
					this.pawn.story.HairColor = hairColor;
					this.pawn.Drawer.renderer.renderTree.SetDirty();
				}
			}
			catch (Exception ex)
			{
				string str = "Error rendering pawn portrait: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			finally
			{
				this.tmpOriginalColors.Clear();
			}
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000AB574 File Offset: 0x000A9774
		private Vector3 GetBodyPos(Vector3 drawLoc, PawnPosture posture, out bool showBody)
		{
			if (posture == PawnPosture.Standing)
			{
				showBody = true;
				return drawLoc;
			}
			Building_Bed building_Bed = this.pawn.CurrentBed();
			Vector3 result;
			if (building_Bed != null && this.pawn.RaceProps.Humanlike)
			{
				showBody = building_Bed.def.building.bed_showSleeperBody;
				AltitudeLayer altLayer = (AltitudeLayer)Mathf.Max((int)building_Bed.def.altitudeLayer, 20);
				Vector3 a = this.pawn.Position.ToVector3ShiftedWithAltitude(altLayer);
				Rot4 rotation = building_Bed.Rotation;
				rotation.AsInt += 2;
				float d = this.BaseHeadOffsetAt(Rot4.South).z + this.pawn.story.bodyType.bedOffset + building_Bed.def.building.bed_pawnDrawOffset;
				Vector3 a2 = rotation.FacingCell.ToVector3();
				result = a - a2 * d;
			}
			else
			{
				showBody = true;
				result = drawLoc;
				IThingHolderWithDrawnPawn thingHolderWithDrawnPawn = this.pawn.ParentHolder as IThingHolderWithDrawnPawn;
				if (thingHolderWithDrawnPawn != null)
				{
					result.y = thingHolderWithDrawnPawn.HeldPawnDrawPos_Y;
				}
				else
				{
					IThingHolderWithDrawnPawn thingHolderWithDrawnPawn2 = this.pawn.ParentHolder.ParentHolder as IThingHolderWithDrawnPawn;
					if (thingHolderWithDrawnPawn2 != null)
					{
						result.y = thingHolderWithDrawnPawn2.HeldPawnDrawPos_Y;
					}
					else if (!this.pawn.Dead && this.pawn.CarriedBy == null && this.pawn.ParentHolder.Isnt<PawnFlyer>())
					{
						result.y = AltitudeLayer.LayingPawn.AltitudeFor();
					}
				}
			}
			Pawn_MindState mindState = this.pawn.mindState;
			bool? flag;
			if (mindState == null)
			{
				flag = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				if (duty == null)
				{
					flag = null;
				}
				else
				{
					DutyDef def = duty.def;
					flag = ((def != null) ? def.drawBodyOverride : null);
				}
			}
			showBody = (flag ?? showBody);
			return result;
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000AB753 File Offset: 0x000A9953
		private bool PawnNeedsHediffMaterial(out PawnRenderFlags renderFlags)
		{
			renderFlags = PawnRenderFlags.None;
			if (this.pawn.IsPsychologicallyInvisible())
			{
				renderFlags |= PawnRenderFlags.Invisible;
				return true;
			}
			return false;
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x000AB76E File Offset: 0x000A996E
		private void RenderPawnInternal(PawnDrawParms parms)
		{
			this.renderTree.Draw(parms);
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x000AB77C File Offset: 0x000A997C
		private PawnDrawParms GetDrawParms(Vector3 rootLoc, float angle, Rot4 bodyFacing, RotDrawMode bodyDrawType, PawnRenderFlags flags)
		{
			PawnDrawParms result = default(PawnDrawParms);
			result.pawn = this.pawn;
			result.matrix = Matrix4x4.TRS(rootLoc + this.pawn.ageTracker.CurLifeStage.bodyDrawOffset, Quaternion.AngleAxis(angle, Vector3.up), Vector3.one);
			result.facing = bodyFacing;
			result.rotDrawMode = bodyDrawType;
			result.posture = this.pawn.GetPosture();
			result.flags = flags;
			result.tint = this.flasher.CurColor.ToTransparent(InvisibilityUtility.GetAlpha(this.pawn));
			result.bed = this.pawn.CurrentBed();
			result.coveredInFoam = this.FirefoamOverlays.coveredInFoam;
			Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
			result.carriedThing = ((carryTracker != null) ? carryTracker.CarriedThing : null);
			result.dead = this.pawn.Dead;
			result.crawling = this.pawn.Crawling;
			result.swimming = this.pawn.Swimming;
			result.statueColor = this.StatueColor;
			return result;
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x000AB8A9 File Offset: 0x000A9AA9
		private Material OverrideMaterialIfNeeded(Material original, PawnRenderFlags flags)
		{
			if (flags.FlagSet(PawnRenderFlags.Cache) || flags.FlagSet(PawnRenderFlags.Portrait))
			{
				return original;
			}
			if (this.pawn.IsPsychologicallyInvisible())
			{
				return InvisibilityMatPool.GetInvisibleMat(original);
			}
			return this.flasher.GetDamagedMat(original);
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x000AB8E0 File Offset: 0x000A9AE0
		private Rot4 RotationForcedByJob()
		{
			Pawn_JobTracker jobs = this.pawn.jobs;
			if (((jobs != null) ? jobs.curDriver : null) != null && this.pawn.jobs.curDriver.ForcedLayingRotation.IsValid)
			{
				return this.pawn.jobs.curDriver.ForcedLayingRotation;
			}
			return Rot4.Invalid;
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x000AB940 File Offset: 0x000A9B40
		public Rot4 LayingFacing()
		{
			Rot4 result = this.RotationForcedByJob();
			if (result.IsValid)
			{
				return result;
			}
			PawnPosture posture = this.pawn.GetPosture();
			if (posture == PawnPosture.LayingOnGroundFaceUp || this.pawn.Deathresting)
			{
				return Rot4.South;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				if (this.pawn.DevelopmentalStage.Baby())
				{
					Pawn_CarryTracker pawn_CarryTracker = this.pawn.ParentHolder as Pawn_CarryTracker;
					if (pawn_CarryTracker != null)
					{
						if (!(pawn_CarryTracker.pawn.Rotation == Rot4.West) && !(pawn_CarryTracker.pawn.Rotation == Rot4.North))
						{
							return Rot4.West;
						}
						return Rot4.East;
					}
				}
				if (posture.FaceUp() && this.pawn.CurrentBed() != null)
				{
					return Rot4.South;
				}
				switch (this.pawn.thingIDNumber % 4)
				{
				case 0:
					return Rot4.South;
				case 1:
					return Rot4.South;
				case 2:
					return Rot4.East;
				case 3:
					return Rot4.West;
				}
			}
			else
			{
				switch (this.pawn.thingIDNumber % 4)
				{
				case 0:
					return Rot4.South;
				case 1:
					return Rot4.East;
				case 2:
					return Rot4.West;
				case 3:
					return Rot4.West;
				}
			}
			return Rot4.Random;
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x000ABA94 File Offset: 0x000A9C94
		public float BodyAngle(PawnRenderFlags flags)
		{
			if (this.pawn.GetPosture() == PawnPosture.Standing)
			{
				return 0f;
			}
			Building_Bed building_Bed = this.pawn.CurrentBed();
			if (building_Bed != null && this.pawn.RaceProps.Humanlike)
			{
				Rot4 rotation = building_Bed.Rotation;
				rotation.AsInt += 2;
				return rotation.AsAngle;
			}
			IThingHolderWithDrawnPawn thingHolderWithDrawnPawn = this.pawn.ParentHolder as IThingHolderWithDrawnPawn;
			if (thingHolderWithDrawnPawn != null)
			{
				return thingHolderWithDrawnPawn.HeldPawnBodyAngle;
			}
			IThingHolderWithDrawnPawn thingHolderWithDrawnPawn2 = this.pawn.ParentHolder.ParentHolder as IThingHolderWithDrawnPawn;
			if (thingHolderWithDrawnPawn2 != null)
			{
				return thingHolderWithDrawnPawn2.HeldPawnBodyAngle;
			}
			Pawn_CarryTracker pawn_CarryTracker = this.pawn.ParentHolder as Pawn_CarryTracker;
			if (pawn_CarryTracker != null)
			{
				Rot4 rotation2 = pawn_CarryTracker.pawn.Rotation;
				return ((rotation2 == Rot4.West || rotation2 == Rot4.North) ? 290f : 70f) + pawn_CarryTracker.pawn.Drawer.renderer.BodyAngle(flags);
			}
			if (this.pawn.Crawling && !flags.FlagSet(PawnRenderFlags.Portrait))
			{
				return PawnRenderUtility.CrawlingBodyAngle(this.pawn.Rotation);
			}
			if (this.pawn.Downed || this.pawn.Dead)
			{
				return this.wiggler.downedAngle;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				return this.LayingFacing().AsAngle;
			}
			if (this.RotationForcedByJob().IsValid)
			{
				return 0f;
			}
			return ((this.pawn.thingIDNumber % 2 == 0) ? Rot4.West : Rot4.East).AsAngle;
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000ABC3C File Offset: 0x000A9E3C
		public Vector3 BaseHeadOffsetAt(Rot4 rotation)
		{
			Vector2 vector = this.pawn.story.bodyType.headOffset * Mathf.Sqrt(this.pawn.ageTracker.CurLifeStage.bodySizeFactor);
			switch (rotation.AsInt)
			{
			case 0:
				return new Vector3(0f, 0f, vector.y);
			case 1:
				return new Vector3(vector.x, 0f, vector.y);
			case 2:
				return new Vector3(0f, 0f, vector.y);
			case 3:
				return new Vector3(-vector.x, 0f, vector.y);
			default:
			{
				string str = "BaseHeadOffsetAt error in ";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null));
				return Vector3.zero;
			}
			}
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000ABD20 File Offset: 0x000A9F20
		public void SetAnimation(AnimationDef animation)
		{
			if (animation == null)
			{
				this.renderTree.currentAnimation = null;
				this.renderTree.animationStartTick = -99999;
			}
			else
			{
				this.renderTree.currentAnimation = animation;
				this.renderTree.animationStartTick = Find.TickManager.TicksGame;
				if (animation.startOnRandomTick)
				{
					this.renderTree.animationStartTick += Rand.RangeSeeded(0, animation.durationTicks, this.pawn.thingIDNumber);
				}
			}
			this.renderTree.SetDirty();
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x000ABDAB File Offset: 0x000A9FAB
		public void SetAllGraphicsDirty()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (this.renderTree.Resolved)
				{
					this.renderTree.SetDirty();
					SilhouetteUtility.NotifyGraphicDirty(this.pawn);
					this.WoundOverlays.ClearCache();
					PortraitsCache.SetDirty(this.pawn);
					GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
				}
			});
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000ABDBE File Offset: 0x000A9FBE
		public void Notify_DamageApplied(DamageInfo dam)
		{
			this.flasher.Notify_DamageApplied(dam);
			this.wiggler.Notify_DamageApplied(dam);
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x000ABDD8 File Offset: 0x000A9FD8
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			this.wiggler.ProcessPostTickVisuals(ticksPassed);
			if (this.renderTree.AnimationFinished)
			{
				this.SetAnimation(null);
			}
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x000ABDFA File Offset: 0x000A9FFA
		public void EffectersTick(bool suspended)
		{
			this.effecters.EffectersTick(suspended);
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x000ABE08 File Offset: 0x000AA008
		private void DrawDebug()
		{
			if (DebugViewSettings.drawDuties && Find.Selector.IsSelected(this.pawn))
			{
				Pawn_MindState mindState = this.pawn.mindState;
				if (mindState == null)
				{
					return;
				}
				PawnDuty duty = mindState.duty;
				if (duty == null)
				{
					return;
				}
				duty.DrawDebug(this.pawn);
			}
		}

		// Token: 0x040016BF RID: 5823
		private readonly Pawn pawn;

		// Token: 0x040016C0 RID: 5824
		public PawnDownedWiggler wiggler;

		// Token: 0x040016C1 RID: 5825
		private PawnHeadOverlays statusOverlays;

		// Token: 0x040016C2 RID: 5826
		private PawnStatusEffecters effecters;

		// Token: 0x040016C3 RID: 5827
		private PawnWoundDrawer woundOverlays;

		// Token: 0x040016C4 RID: 5828
		private PawnFirefoamDrawer firefoamOverlays;

		// Token: 0x040016C5 RID: 5829
		private PawnShamblerScarDrawer shamblerScarDrawer;

		// Token: 0x040016C6 RID: 5830
		private PawnScariaSoreDrawer scariaSoreDrawer;

		// Token: 0x040016C7 RID: 5831
		private Graphic_Shadow shadowGraphic;

		// Token: 0x040016C8 RID: 5832
		public DamageFlasher flasher;

		// Token: 0x040016C9 RID: 5833
		public PawnRenderTree renderTree;

		// Token: 0x040016CA RID: 5834
		private Graphic silhouetteGraphic;

		// Token: 0x040016CB RID: 5835
		private Vector3 silhouettePos;

		// Token: 0x040016CC RID: 5836
		private ThingDef statueStuff;

		// Token: 0x040016CD RID: 5837
		private Color? statuePaintColor;

		// Token: 0x040016CE RID: 5838
		private const float CarriedPawnDrawAngle = 70f;

		// Token: 0x040016CF RID: 5839
		private const float CachedPawnTextureMinCameraZoom = 18f;

		// Token: 0x040016D0 RID: 5840
		private PawnRenderer.PreRenderResults results;

		// Token: 0x040016D1 RID: 5841
		private readonly Dictionary<Apparel, ValueTuple<Color, bool>> tmpOriginalColors = new Dictionary<Apparel, ValueTuple<Color, bool>>();

		// Token: 0x020026A3 RID: 9891
		private struct PreRenderResults
		{
			// Token: 0x04009E2A RID: 40490
			public bool valid;

			// Token: 0x04009E2B RID: 40491
			public bool draw;

			// Token: 0x04009E2C RID: 40492
			public bool useCached;

			// Token: 0x04009E2D RID: 40493
			public bool showBody;

			// Token: 0x04009E2E RID: 40494
			public float bodyAngle;

			// Token: 0x04009E2F RID: 40495
			public PawnPosture posture;

			// Token: 0x04009E30 RID: 40496
			public Vector3 bodyPos;

			// Token: 0x04009E31 RID: 40497
			public PawnDrawParms parms;
		}
	}
}
