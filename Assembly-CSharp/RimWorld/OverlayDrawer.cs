using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A65 RID: 6757
	[StaticConstructorOnStartup]
	public class OverlayDrawer
	{
		// Token: 0x0600A9BA RID: 43450 RVA: 0x003C2B30 File Offset: 0x003C0D30
		public ThingOverlaysHandle GetOverlaysHandle(Thing thing)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			ThingOverlaysHandle thingOverlaysHandle;
			if (!this.overlayHandles.TryGetValue(thing, out thingOverlaysHandle))
			{
				thingOverlaysHandle = new ThingOverlaysHandle(this, thing);
				this.overlayHandles.Add(thing, thingOverlaysHandle);
			}
			return thingOverlaysHandle;
		}

		// Token: 0x0600A9BB RID: 43451 RVA: 0x003C2B70 File Offset: 0x003C0D70
		public void DisposeHandle(Thing thing)
		{
			ThingOverlaysHandle thingOverlaysHandle;
			if (this.overlayHandles.TryGetValue(thing, out thingOverlaysHandle))
			{
				thingOverlaysHandle.Dispose();
			}
			this.overlayHandles.Remove(thing);
		}

		// Token: 0x0600A9BC RID: 43452 RVA: 0x003C2BA0 File Offset: 0x003C0DA0
		public OverlayHandle Enable(Thing thing, OverlayTypes types)
		{
			return this.GetOverlaysHandle(thing).Enable(types);
		}

		// Token: 0x0600A9BD RID: 43453 RVA: 0x003C2BAF File Offset: 0x003C0DAF
		public void Disable(Thing thing, ref OverlayHandle? handle)
		{
			this.GetOverlaysHandle(thing).Disable(ref handle);
		}

		// Token: 0x0600A9BE RID: 43454 RVA: 0x003C2BC0 File Offset: 0x003C0DC0
		public void DrawOverlay(Thing t, OverlayTypes overlayType)
		{
			if (overlayType == OverlayTypes.None || WorldComponent_GravshipController.CutsceneInProgress)
			{
				return;
			}
			OverlayTypes overlayTypes;
			if (this.overlaysToDraw.TryGetValue(t, out overlayTypes))
			{
				this.overlaysToDraw[t] = (overlayTypes | overlayType);
				return;
			}
			this.overlaysToDraw.Add(t, overlayType);
		}

		// Token: 0x0600A9BF RID: 43455 RVA: 0x003C2C08 File Offset: 0x003C0E08
		public void DrawAllOverlays()
		{
			if (WorldComponent_GravshipController.CutsceneInProgress)
			{
				return;
			}
			try
			{
				foreach (KeyValuePair<Thing, ThingOverlaysHandle> keyValuePair in this.overlayHandles)
				{
					if (!keyValuePair.Key.Fogged())
					{
						this.DrawOverlay(keyValuePair.Key, keyValuePair.Value.OverlayTypes);
					}
				}
				foreach (KeyValuePair<Thing, OverlayTypes> keyValuePair2 in this.overlaysToDraw)
				{
					this.curOffset = Vector3.zero;
					Thing key = keyValuePair2.Key;
					OverlayTypes value = keyValuePair2.Value;
					if ((value & OverlayTypes.BurningWick) != OverlayTypes.None)
					{
						this.RenderBurningWick(key);
					}
					else
					{
						OverlayTypes overlayTypes = OverlayTypes.NeedsPower | OverlayTypes.PowerOff;
						int bitCountOf = Gen.GetBitCountOf((long)(value & overlayTypes));
						float num = this.StackOffsetFor(key);
						switch (bitCountOf)
						{
						case 1:
							this.curOffset = Vector3.zero;
							break;
						case 2:
							this.curOffset = new Vector3(-0.5f * num, 0f, 0f);
							break;
						case 3:
							this.curOffset = new Vector3(-1.5f * num, 0f, 0f);
							break;
						}
						if ((value & OverlayTypes.NeedsPower) != OverlayTypes.None)
						{
							this.RenderNeedsPowerOverlay(key);
						}
						if ((value & OverlayTypes.PowerOff) != OverlayTypes.None)
						{
							this.RenderPowerOffOverlay(key);
						}
						if ((value & OverlayTypes.BrokenDown) != OverlayTypes.None)
						{
							this.RenderBrokenDownOverlay(key);
						}
						if ((value & OverlayTypes.OutOfFuel) != OverlayTypes.None)
						{
							this.RenderOutOfFuelOverlay(key);
						}
					}
					if ((value & OverlayTypes.ForbiddenBig) != OverlayTypes.None)
					{
						this.RenderForbiddenBigOverlay(key);
					}
					if ((value & OverlayTypes.Forbidden) != OverlayTypes.None)
					{
						this.RenderForbiddenOverlay(key);
					}
					if ((value & OverlayTypes.ForbiddenRefuel) != OverlayTypes.None)
					{
						this.RenderForbiddenRefuelOverlay(key);
					}
					if ((value & OverlayTypes.QuestionMark) != OverlayTypes.None)
					{
						this.RenderQuestionMarkOverlay(key);
					}
					if ((value & OverlayTypes.SelfShutdown) != OverlayTypes.None && ModsConfig.BiotechActive)
					{
						this.RenderRechargineOverlay(key);
					}
					if ((value & OverlayTypes.ForbiddenAtomizer) != OverlayTypes.None && ModsConfig.BiotechActive)
					{
						this.RenderForbiddenAtomizerOverlay(key);
					}
				}
			}
			finally
			{
				this.overlaysToDraw.Clear();
			}
			this.drawBatch.Flush(true);
		}

		// Token: 0x0600A9C0 RID: 43456 RVA: 0x003C2E68 File Offset: 0x003C1068
		private float StackOffsetFor(Thing t)
		{
			return (float)t.RotatedSize.x * 0.25f;
		}

		// Token: 0x0600A9C1 RID: 43457 RVA: 0x003C2E7C File Offset: 0x003C107C
		private void RenderNeedsPowerOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.NeedsPowerMat, 2, true);
		}

		// Token: 0x0600A9C2 RID: 43458 RVA: 0x003C2E8C File Offset: 0x003C108C
		private void RenderPowerOffOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.PowerOffMat, 3, true);
		}

		// Token: 0x0600A9C3 RID: 43459 RVA: 0x003C2E9C File Offset: 0x003C109C
		private void RenderBrokenDownOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.BrokenDownMat, 4, true);
		}

		// Token: 0x0600A9C4 RID: 43460 RVA: 0x003C2EAC File Offset: 0x003C10AC
		private void RenderRechargineOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.21951221f;
			this.RenderPulsingOverlayInternal(t, OverlayDrawer.SelfShutdownMat, drawPos, MeshPool.plane05);
		}

		// Token: 0x0600A9C5 RID: 43461 RVA: 0x003C2EE4 File Offset: 0x003C10E4
		private void RenderOutOfFuelOverlay(Thing t)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			Material mat = MaterialPool.MatFrom((compRefuelable != null) ? compRefuelable.Props.FuelIcon : ThingDefOf.Chemfuel.uiIcon, ShaderDatabase.MetaOverlay, Color.white);
			this.RenderPulsingOverlay(t, mat, 5, false);
			this.RenderPulsingOverlay(t, OverlayDrawer.OutOfFuelMat, 6, true);
		}

		// Token: 0x0600A9C6 RID: 43462 RVA: 0x003C2F3C File Offset: 0x003C113C
		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, bool incrementOffset = true)
		{
			Mesh plane = MeshPool.plane08;
			this.RenderPulsingOverlay(thing, mat, altInd, plane, incrementOffset);
		}

		// Token: 0x0600A9C7 RID: 43463 RVA: 0x003C2F5C File Offset: 0x003C115C
		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, Mesh mesh, bool incrementOffset = true)
		{
			Vector3 vector = thing.TrueCenter();
			vector.y = OverlayDrawer.BaseAlt + 0.03658537f * (float)altInd;
			vector += this.curOffset;
			if (thing.def.building != null && thing.def.building.isAttachment)
			{
				vector += (thing.Rotation.AsVector2 * 0.5f).ToVector3();
			}
			vector.y = Mathf.Min(vector.y, Find.Camera.transform.position.y - 0.1f);
			if (incrementOffset)
			{
				this.curOffset.x = this.curOffset.x + this.StackOffsetFor(thing);
			}
			this.RenderPulsingOverlayInternal(thing, mat, vector, mesh);
		}

		// Token: 0x0600A9C8 RID: 43464 RVA: 0x003C3028 File Offset: 0x003C1228
		private void RenderPulsingOverlayInternal(Thing thing, Material mat, Vector3 drawPos, Mesh mesh)
		{
			float num = ((float)Math.Sin((double)((Time.realtimeSinceStartup + 397f * (float)(thing.thingIDNumber % 571)) * 4f)) + 1f) * 0.5f;
			num = 0.3f + num * 0.7f;
			Material material = FadedMaterialPool.FadedVersionOf(mat, num);
			this.drawBatch.DrawMesh(mesh, Matrix4x4.TRS(drawPos, Quaternion.identity, Vector3.one), material, 0, true);
		}

		// Token: 0x0600A9C9 RID: 43465 RVA: 0x003C30A0 File Offset: 0x003C12A0
		private void RenderForbiddenRefuelOverlay(Thing t)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			Material material = MaterialPool.MatFrom((compRefuelable != null) ? compRefuelable.Props.FuelIcon : ThingDefOf.Chemfuel.uiIcon, ShaderDatabase.MetaOverlayDesaturated, Color.white);
			Vector3 vector = t.TrueCenter();
			vector.y = OverlayDrawer.BaseAlt + 0.18292683f;
			new Vector3(vector.x, vector.y + 0.03658537f, vector.z);
			this.drawBatch.DrawMesh(MeshPool.plane08, Matrix4x4.TRS(vector, Quaternion.identity, Vector3.one), material, 0, true);
			this.drawBatch.DrawMesh(MeshPool.plane08, Matrix4x4.TRS(vector, Quaternion.identity, Vector3.one), OverlayDrawer.ForbiddenMat, 0, true);
		}

		// Token: 0x0600A9CA RID: 43466 RVA: 0x003C3160 File Offset: 0x003C1360
		private void RenderForbiddenAtomizerOverlay(Thing t)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			t.TryGetComp<CompAtomizer>();
			Material material = MaterialPool.MatFrom(ThingDefOf.Wastepack.uiIcon, ShaderDatabase.MetaOverlayDesaturated, Color.white);
			Vector3 pos = t.TrueCenter();
			pos.y = OverlayDrawer.BaseAlt + 0.18292683f;
			this.drawBatch.DrawMesh(MeshPool.plane08, Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one), material, 0, true);
			this.drawBatch.DrawMesh(MeshPool.plane08, Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one), OverlayDrawer.ForbiddenMat, 0, true);
		}

		// Token: 0x0600A9CB RID: 43467 RVA: 0x003C31FC File Offset: 0x003C13FC
		private void RenderForbiddenOverlay(Thing t)
		{
			Vector3 vector = t.DrawPos;
			ThingDef thingDef = t.def.entityDefToBuild as ThingDef;
			bool? flag;
			if (thingDef == null)
			{
				flag = null;
			}
			else
			{
				BuildingProperties building = thingDef.building;
				flag = ((building != null) ? new bool?(building.isAttachment) : null);
			}
			bool? flag2 = flag;
			bool flag3;
			if (flag2 == null)
			{
				BuildingProperties building2 = t.def.building;
				flag3 = (building2 != null && building2.isAttachment);
			}
			else
			{
				flag3 = flag2.GetValueOrDefault();
			}
			if (flag3)
			{
				vector += (t.Rotation.AsVector2 * 0.5f).ToVector3();
			}
			if (t.RotatedSize.z == 1)
			{
				vector.z -= OverlayDrawer.SingleCellForbiddenOffset;
			}
			else
			{
				vector.z -= (float)t.RotatedSize.z * 0.3f;
			}
			vector.y = OverlayDrawer.BaseAlt + 0.14634147f;
			this.drawBatch.DrawMesh(MeshPool.plane05, Matrix4x4.TRS(vector, Quaternion.identity, Vector3.one), OverlayDrawer.ForbiddenMat, 0, true);
		}

		// Token: 0x0600A9CC RID: 43468 RVA: 0x003C3314 File Offset: 0x003C1514
		private void RenderForbiddenBigOverlay(Thing t)
		{
			Vector3 vector = t.DrawPos;
			ThingDef thingDef = t.def.entityDefToBuild as ThingDef;
			bool? flag;
			if (thingDef == null)
			{
				flag = null;
			}
			else
			{
				BuildingProperties building = thingDef.building;
				flag = ((building != null) ? new bool?(building.isAttachment) : null);
			}
			bool? flag2 = flag;
			bool flag3;
			if (flag2 == null)
			{
				BuildingProperties building2 = t.def.building;
				flag3 = (building2 != null && building2.isAttachment);
			}
			else
			{
				flag3 = flag2.GetValueOrDefault();
			}
			if (flag3)
			{
				vector += (t.Rotation.AsVector2 * 0.5f).ToVector3();
			}
			vector.y = OverlayDrawer.BaseAlt + 0.14634147f;
			this.drawBatch.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(vector, Quaternion.identity, Vector3.one), OverlayDrawer.ForbiddenMat, 0, true);
		}

		// Token: 0x0600A9CD RID: 43469 RVA: 0x003C33F0 File Offset: 0x003C15F0
		private void RenderBurningWick(Thing parent)
		{
			Material material;
			if ((parent.thingIDNumber + Find.TickManager.TicksGame) % 6 < 3)
			{
				material = OverlayDrawer.WickMaterialA;
			}
			else
			{
				material = OverlayDrawer.WickMaterialB;
			}
			Vector3 drawPos = parent.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.18292683f;
			this.drawBatch.DrawMesh(MeshPool.plane20, Matrix4x4.TRS(drawPos, Quaternion.identity, Vector3.one), material, 0, true);
		}

		// Token: 0x0600A9CE RID: 43470 RVA: 0x003C3460 File Offset: 0x003C1660
		private void RenderQuestionMarkOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.21951221f;
			if (t is Pawn)
			{
				drawPos.x += (float)t.def.size.x - 0.52f;
				drawPos.z += (float)t.def.size.z - 0.45f;
			}
			this.RenderPulsingOverlayInternal(t, OverlayDrawer.QuestionMarkMat, drawPos, MeshPool.plane05);
		}

		// Token: 0x0400634C RID: 25420
		private Dictionary<Thing, OverlayTypes> overlaysToDraw = new Dictionary<Thing, OverlayTypes>();

		// Token: 0x0400634D RID: 25421
		private Dictionary<Thing, ThingOverlaysHandle> overlayHandles = new Dictionary<Thing, ThingOverlaysHandle>();

		// Token: 0x0400634E RID: 25422
		private Vector3 curOffset;

		// Token: 0x0400634F RID: 25423
		private DrawBatch drawBatch = new DrawBatch();

		// Token: 0x04006350 RID: 25424
		private static readonly Material ForbiddenMat = MaterialPool.MatFrom("Things/Special/ForbiddenOverlay", ShaderDatabase.MetaOverlay);

		// Token: 0x04006351 RID: 25425
		private static Material NeedsPowerMat = MaterialPool.MatFrom("UI/Overlays/NeedsPower", ShaderDatabase.MetaOverlay);

		// Token: 0x04006352 RID: 25426
		private static readonly Material PowerOffMat = MaterialPool.MatFrom("UI/Overlays/PowerOff", ShaderDatabase.MetaOverlay);

		// Token: 0x04006353 RID: 25427
		private static readonly Material QuestionMarkMat = MaterialPool.MatFrom("UI/Overlays/QuestionMark", ShaderDatabase.MetaOverlay);

		// Token: 0x04006354 RID: 25428
		private static readonly Material BrokenDownMat = MaterialPool.MatFrom("UI/Overlays/BrokenDown", ShaderDatabase.MetaOverlay);

		// Token: 0x04006355 RID: 25429
		private static readonly Material OutOfFuelMat = MaterialPool.MatFrom("UI/Overlays/OutOfFuel", ShaderDatabase.MetaOverlay);

		// Token: 0x04006356 RID: 25430
		private static readonly Material WickMaterialA = MaterialPool.MatFrom("Things/Special/BurningWickA", ShaderDatabase.MetaOverlay);

		// Token: 0x04006357 RID: 25431
		private static readonly Material WickMaterialB = MaterialPool.MatFrom("Things/Special/BurningWickB", ShaderDatabase.MetaOverlay);

		// Token: 0x04006358 RID: 25432
		private static readonly Material SelfShutdownMat = MaterialPool.MatFrom("UI/Overlays/SelfShutdown", ShaderDatabase.MetaOverlay);

		// Token: 0x04006359 RID: 25433
		private const int AltitudeIndex_Forbidden = 4;

		// Token: 0x0400635A RID: 25434
		private const int AltitudeIndex_BurningWick = 5;

		// Token: 0x0400635B RID: 25435
		private const int AltitudeIndex_QuestionMark = 6;

		// Token: 0x0400635C RID: 25436
		private static float SingleCellForbiddenOffset = 0.3f;

		// Token: 0x0400635D RID: 25437
		public const float PulseFrequency = 4f;

		// Token: 0x0400635E RID: 25438
		public const float PulseAmplitude = 0.7f;

		// Token: 0x0400635F RID: 25439
		private static readonly float BaseAlt = AltitudeLayer.MetaOverlays.AltitudeFor();

		// Token: 0x04006360 RID: 25440
		private const float StackOffsetMultipiler = 0.25f;
	}
}
