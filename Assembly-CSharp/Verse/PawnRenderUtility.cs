using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000396 RID: 918
	public static class PawnRenderUtility
	{
		// Token: 0x06001BF0 RID: 7152 RVA: 0x000ACFF1 File Offset: 0x000AB1F1
		public static void ResetStaticData()
		{
			PawnRenderUtility.overlayMats.Clear();
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000AD000 File Offset: 0x000AB200
		public static bool RenderAsPack(this Apparel apparel)
		{
			return apparel.def.apparel.LastLayer.IsUtilityLayer && (apparel.def.apparel.wornGraphicData == null || apparel.def.apparel.wornGraphicData.renderUtilityAsPack);
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x000AD050 File Offset: 0x000AB250
		public static Color GetRottenColor(Color color)
		{
			return new Color(color.r * 0.25f + PawnRenderUtility.BaseRottenColor.r * 0.75f, color.g * 0.25f + PawnRenderUtility.BaseRottenColor.g * 0.75f, color.b * 0.25f + PawnRenderUtility.BaseRottenColor.b * 0.75f, color.a);
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x000AD0C0 File Offset: 0x000AB2C0
		public static void DrawCarriedWeapon(ThingWithComps weapon, Vector3 drawPos, Rot4 facing, float equipmentDrawDistanceFactor)
		{
			int num = 143;
			switch (facing.AsInt)
			{
			case 0:
				drawPos += PawnRenderUtility.EqLocNorth * equipmentDrawDistanceFactor;
				break;
			case 1:
				drawPos += PawnRenderUtility.EqLocEast * equipmentDrawDistanceFactor;
				break;
			case 2:
				drawPos += PawnRenderUtility.EqLocSouth * equipmentDrawDistanceFactor;
				break;
			case 3:
				drawPos += PawnRenderUtility.EqLocWest * equipmentDrawDistanceFactor;
				num = 217;
				break;
			}
			PawnRenderUtility.DrawEquipmentAiming(weapon, drawPos, (float)num);
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000AD154 File Offset: 0x000AB354
		public static void DrawEquipmentAiming(Thing eq, Vector3 drawLoc, float aimAngle)
		{
			float num = aimAngle - 90f;
			Mesh mesh;
			if (aimAngle > 20f && aimAngle < 160f)
			{
				mesh = MeshPool.plane10;
				num += eq.def.equippedAngleOffset;
			}
			else if (aimAngle > 200f && aimAngle < 340f)
			{
				mesh = MeshPool.plane10Flip;
				num -= 180f;
				num -= eq.def.equippedAngleOffset;
			}
			else
			{
				mesh = MeshPool.plane10;
				num += eq.def.equippedAngleOffset;
			}
			num %= 360f;
			CompEquippable compEquippable = eq.TryGetComp<CompEquippable>();
			if (compEquippable != null)
			{
				Vector3 b;
				float num2;
				EquipmentUtility.Recoil(eq.def, EquipmentUtility.GetRecoilVerb(compEquippable.AllVerbs), out b, out num2, aimAngle);
				drawLoc += b;
				num += num2;
			}
			Graphic_StackCount graphic_StackCount = eq.Graphic as Graphic_StackCount;
			Material material;
			if (graphic_StackCount != null)
			{
				material = graphic_StackCount.SubGraphicForStackCount(1, eq.def).MatSingleFor(eq);
			}
			else
			{
				material = eq.Graphic.MatSingleFor(eq);
			}
			Vector3 s = new Vector3(eq.Graphic.drawSize.x, 0f, eq.Graphic.drawSize.y);
			Matrix4x4 matrix = Matrix4x4.TRS(drawLoc, Quaternion.AngleAxis(num, Vector3.up), s);
			Graphics.DrawMesh(mesh, matrix, material, 0);
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000AD290 File Offset: 0x000AB490
		public static float CrawlingHeadAngle(Rot4 rotation)
		{
			switch (rotation.AsInt)
			{
			case 0:
				return 0f;
			case 1:
				return -50f;
			case 2:
				return 0f;
			case 3:
				return 50f;
			default:
			{
				string str = "Invalid rotation: ";
				Rot4 rot = rotation;
				Log.Warning(str + rot.ToString());
				return 0f;
			}
			}
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x000AD2F8 File Offset: 0x000AB4F8
		public static float CrawlingBodyAngle(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return 0f;
			case 1:
				return 75f;
			case 2:
				return 180f;
			case 3:
				return 285f;
			default:
			{
				string str = "Invalid rotation: ";
				Rot4 rot2 = rot;
				Log.Warning(str + rot2.ToString());
				return 0f;
			}
			}
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000AD360 File Offset: 0x000AB560
		public static void DrawCarriedThing(Pawn pawn, Vector3 drawLoc, Thing carriedThing)
		{
			bool flip;
			PawnRenderUtility.CalculateCarriedDrawPos(pawn, carriedThing, ref drawLoc, out flip);
			carriedThing.DrawNowAt(drawLoc, flip);
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000AD380 File Offset: 0x000AB580
		public static void CalculateCarriedDrawPos(Pawn pawn, Thing carriedThing, ref Vector3 carryDrawPos, out bool flip)
		{
			flip = false;
			if (pawn.CurJob != null && pawn.jobs.curDriver.ModifyCarriedThingDrawPos(ref carryDrawPos, ref flip))
			{
				return;
			}
			CompPushable compPushable = carriedThing.TryGetComp<CompPushable>();
			if (compPushable != null)
			{
				float degrees;
				if (pawn.pather.MovingNow)
				{
					degrees = (pawn.pather.nextCell - pawn.Position).ToVector3().ToAngleFlat();
				}
				else
				{
					degrees = pawn.Rotation.AsAngle - 90f;
				}
				Vector2 vector = new Vector2(compPushable.Props.offsetDistance, 0f).RotatedBy(degrees);
				Vector3 target = new Vector3(vector.x, 0f, -vector.y);
				float deltaTime = Find.TickManager.TickRateMultiplier / 60f;
				if (!Find.TickManager.Paused)
				{
					compPushable.drawPos = Vector3.SmoothDamp(compPushable.drawPos, target, ref compPushable.drawVel, compPushable.Props.smoothTime, 10f, deltaTime);
				}
				carryDrawPos += compPushable.drawPos;
				if (compPushable.drawPos.y < 0f)
				{
					carryDrawPos.y = pawn.DrawPos.y + PawnRenderUtility.AltitudeForLayer(-10f);
					return;
				}
			}
			else
			{
				Rot4 rotation = pawn.Rotation;
				Vector3 baseCarriedOffset = PawnRenderUtility.BaseCarriedOffset;
				switch (rotation.AsInt)
				{
				case 0:
					baseCarriedOffset.z = 0f;
					break;
				case 1:
					baseCarriedOffset.x = 0.18f;
					break;
				case 3:
					baseCarriedOffset.x = -0.18f;
					break;
				}
				carryDrawPos += baseCarriedOffset;
				if (pawn.DevelopmentalStage == DevelopmentalStage.Child)
				{
					carryDrawPos.z -= 0.1f;
				}
				if (pawn.Rotation == Rot4.North)
				{
					carryDrawPos.y -= 0.03658537f;
					return;
				}
				carryDrawPos.y += 0.03658537f;
			}
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x000AD580 File Offset: 0x000AB780
		public static void DrawEquipmentAndApparelExtras(Pawn pawn, Vector3 drawPos, Rot4 facing, PawnRenderFlags flags)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			if (((equipment != null) ? equipment.Primary : null) != null)
			{
				Job curJob = pawn.CurJob;
				bool flag;
				if (curJob == null)
				{
					flag = false;
				}
				else
				{
					JobDef def = curJob.def;
					bool? flag2 = (def != null) ? new bool?(def.neverShowWeapon) : null;
					bool flag3 = false;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (flag)
				{
					Pawn_StanceTracker stances = pawn.stances;
					Stance_Busy stance_Busy = ((stances != null) ? stances.curStance : null) as Stance_Busy;
					float equipmentDrawDistanceFactor = pawn.ageTracker.CurLifeStage.equipmentDrawDistanceFactor;
					float num = 0f;
					if (!flags.HasFlag(PawnRenderFlags.NeverAimWeapon) && stance_Busy != null && !stance_Busy.neverAimWeapon && stance_Busy.focusTarg.IsValid)
					{
						Vector3 a = stance_Busy.focusTarg.HasThing ? stance_Busy.focusTarg.Thing.DrawPos : stance_Busy.focusTarg.Cell.ToVector3Shifted();
						if ((a - pawn.DrawPos).MagnitudeHorizontalSquared() > 0.001f)
						{
							num = (a - pawn.DrawPos).AngleFlat();
						}
						Verb currentEffectiveVerb = pawn.CurrentEffectiveVerb;
						if (currentEffectiveVerb != null && currentEffectiveVerb.AimAngleOverride != null)
						{
							num = currentEffectiveVerb.AimAngleOverride.Value;
						}
						drawPos += new Vector3(0f, 0f, 0.4f + pawn.equipment.Primary.def.equippedDistanceOffset).RotatedBy(num) * equipmentDrawDistanceFactor;
						PawnRenderUtility.DrawEquipmentAiming(pawn.equipment.Primary, drawPos, num);
					}
					else if (PawnRenderUtility.CarryWeaponOpenly(pawn))
					{
						PawnRenderUtility.DrawCarriedWeapon(pawn.equipment.Primary, drawPos, facing, equipmentDrawDistanceFactor);
					}
				}
			}
			if (pawn.apparel != null)
			{
				foreach (Apparel apparel in pawn.apparel.WornApparel)
				{
					apparel.DrawWornExtras();
				}
			}
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x000AD7AC File Offset: 0x000AB9AC
		public static float AltitudeForLayer(float layer)
		{
			return Mathf.Clamp(layer, -10f, 100f) * 0.0003658537f;
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x000AD7C4 File Offset: 0x000AB9C4
		public static bool CarryWeaponOpenly(Pawn pawn)
		{
			Pawn_CarryTracker carryTracker = pawn.carryTracker;
			if (((carryTracker != null) ? carryTracker.CarriedThing : null) != null)
			{
				return false;
			}
			if (pawn.Drafted)
			{
				return true;
			}
			if (pawn.CurJob != null && pawn.CurJob.def.alwaysShowWeapon)
			{
				return true;
			}
			Pawn_MindState mindState = pawn.mindState;
			if (((mindState != null) ? mindState.duty : null) != null && pawn.mindState.duty.def.alwaysShowWeapon)
			{
				return true;
			}
			Lord lord = pawn.GetLord();
			return (((lord != null) ? lord.LordJob : null) != null && lord.LordJob.AlwaysShowWeapon) || pawn.CompsWantHoldWeapon();
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000AD869 File Offset: 0x000ABA69
		public static void SetMatPropBlockOverlay(MaterialPropertyBlock block, Color color, float opacity = 0.5f)
		{
			block.SetColor(ShaderPropertyIDs.OverlayColor, color);
			block.SetFloat(ShaderPropertyIDs.OverlayOpacity, opacity);
		}

		// Token: 0x040016F4 RID: 5876
		private static readonly Color BaseRottenColor = new Color(0.29f, 0.25f, 0.22f);

		// Token: 0x040016F5 RID: 5877
		public static readonly Color DessicatedColorInsect = new Color(0.8f, 0.8f, 0.8f);

		// Token: 0x040016F6 RID: 5878
		private static readonly Vector3 BaseCarriedOffset = new Vector3(0f, 0f, -0.1f);

		// Token: 0x040016F7 RID: 5879
		private static readonly Vector3 EqLocNorth = new Vector3(0f, 0f, -0.11f);

		// Token: 0x040016F8 RID: 5880
		private static readonly Vector3 EqLocEast = new Vector3(0.22f, 0f, -0.22f);

		// Token: 0x040016F9 RID: 5881
		private static readonly Vector3 EqLocSouth = new Vector3(0f, 0f, -0.22f);

		// Token: 0x040016FA RID: 5882
		private static readonly Vector3 EqLocWest = new Vector3(-0.22f, 0f, -0.22f);

		// Token: 0x040016FB RID: 5883
		public const float Layer_Carried = 90f;

		// Token: 0x040016FC RID: 5884
		public const float Layer_Carried_Behind = -10f;

		// Token: 0x040016FD RID: 5885
		public static readonly List<PawnRenderNodeProperties> EmptyRenderNodeProperties = new List<PawnRenderNodeProperties>();

		// Token: 0x040016FE RID: 5886
		private static readonly Dictionary<Material, Material> overlayMats = new Dictionary<Material, Material>();
	}
}
