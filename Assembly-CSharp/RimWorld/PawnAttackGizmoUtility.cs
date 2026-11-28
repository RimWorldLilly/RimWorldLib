using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B7 RID: 5303
	public static class PawnAttackGizmoUtility
	{
		// Token: 0x060081CB RID: 33227 RVA: 0x002E5150 File Offset: 0x002E3350
		public static IEnumerable<Gizmo> GetAttackGizmos(Pawn pawn)
		{
			PawnAttackGizmoUtility.<GetAttackGizmos>d__0 <GetAttackGizmos>d__ = new PawnAttackGizmoUtility.<GetAttackGizmos>d__0(-2);
			<GetAttackGizmos>d__.<>3__pawn = pawn;
			return <GetAttackGizmos>d__;
		}

		// Token: 0x060081CC RID: 33228 RVA: 0x002E5160 File Offset: 0x002E3360
		public static bool CanShowEquipmentGizmos()
		{
			return !PawnAttackGizmoUtility.AtLeastTwoSelectedPlayerPawnsHaveDifferentWeapons();
		}

		// Token: 0x060081CD RID: 33229 RVA: 0x002E516A File Offset: 0x002E336A
		private static bool ShouldUseSquadAttackGizmo()
		{
			return PawnAttackGizmoUtility.AtLeastOneSelectedPlayerPawnHasRangedWeapon() && PawnAttackGizmoUtility.AtLeastTwoSelectedPlayerPawnsHaveDifferentWeapons();
		}

		// Token: 0x060081CE RID: 33230 RVA: 0x002E517A File Offset: 0x002E337A
		private static bool CanOrderPlayerPawn(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled || pawn.IsColonyMechPlayerControlled || pawn.IsColonySubhumanPlayerControlled;
		}

		// Token: 0x060081CF RID: 33231 RVA: 0x002E5194 File Offset: 0x002E3394
		private static Gizmo GetSquadAttackGizmo(Pawn pawn)
		{
			Command_Target command_Target = new Command_Target();
			command_Target.defaultLabel = "CommandSquadAttack".Translate();
			command_Target.defaultDesc = "CommandSquadAttackDesc".Translate();
			command_Target.hotKey = KeyBindingDefOf.Misc1;
			command_Target.icon = TexCommand.SquadAttack;
			command_Target.targetingParams = TargetingParameters.ForAttackAny();
			command_Target.targetingParams.canTargetLocations = PawnAttackGizmoUtility.AllSelectedPlayerPawnsCanTargetLocations();
			string str;
			if (FloatMenuUtility.GetAttackAction(pawn, LocalTargetInfo.Invalid, out str) == null)
			{
				command_Target.Disable(str.CapitalizeFirst() + ".");
			}
			command_Target.action = delegate(LocalTargetInfo target)
			{
				foreach (Pawn pawn2 in Find.Selector.SelectedObjects.Where(delegate(object x)
				{
					Pawn pawn3 = x as Pawn;
					return pawn3 != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn3) && pawn3.Drafted;
				}).Cast<Pawn>())
				{
					string text;
					Action attackAction = FloatMenuUtility.GetAttackAction(pawn2, target, out text);
					if (attackAction != null)
					{
						attackAction();
					}
					else if (!text.NullOrEmpty())
					{
						Messages.Message(text, target.Thing, MessageTypeDefOf.RejectInput, false);
					}
				}
			};
			command_Target.onUpdate = delegate(LocalTargetInfo target)
			{
				foreach (Pawn pawn2 in Find.Selector.SelectedObjects.Where(delegate(object x)
				{
					Pawn pawn3 = x as Pawn;
					return pawn3 != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn3) && pawn3.Drafted;
				}).Cast<Pawn>())
				{
					ThingWithComps thingWithComps = pawn2.equipment.AllEquipmentListForReading.FirstOrDefault((ThingWithComps w) => w.def.IsRangedWeapon);
					CompEquippable compEquippable;
					if (thingWithComps != null && thingWithComps.TryGetComp(out compEquippable))
					{
						Verb verb = compEquippable.AllVerbs.FirstOrDefault((Verb v) => v.verbProps.hasStandardCommand);
						if (verb != null)
						{
							verb.verbProps.DrawRadiusRing(pawn2.Position, verb);
						}
					}
				}
			};
			return command_Target;
		}

		// Token: 0x060081D0 RID: 33232 RVA: 0x002E5272 File Offset: 0x002E3472
		private static bool ShouldUseMeleeAttackGizmo(Pawn pawn)
		{
			return pawn.Drafted && (PawnAttackGizmoUtility.AtLeastOneSelectedPlayerPawnHasRangedWeapon() || PawnAttackGizmoUtility.AtLeastOneSelectedPlayerPawnHasNoWeapon() || PawnAttackGizmoUtility.AtLeastTwoSelectedPlayerPawnsHaveDifferentWeapons());
		}

		// Token: 0x060081D1 RID: 33233 RVA: 0x002E5294 File Offset: 0x002E3494
		public static Gizmo GetMeleeAttackGizmo(Pawn pawn)
		{
			Command_Target command_Target = new Command_Target();
			command_Target.defaultLabel = "CommandMeleeAttack".Translate();
			command_Target.defaultDesc = "CommandMeleeAttackDesc".Translate();
			command_Target.targetingParams = TargetingParameters.ForAttackAny();
			command_Target.hotKey = KeyBindingDefOf.Misc2;
			command_Target.icon = TexCommand.AttackMelee;
			string str;
			if (FloatMenuUtility.GetMeleeAttackAction(pawn, LocalTargetInfo.Invalid, out str, false) == null)
			{
				command_Target.Disable(str.CapitalizeFirst() + ".");
			}
			command_Target.action = delegate(LocalTargetInfo target)
			{
				foreach (Pawn pawn2 in Find.Selector.SelectedObjects.Where(delegate(object x)
				{
					Pawn pawn3 = x as Pawn;
					return pawn3 != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn3) && pawn3.Drafted;
				}).Cast<Pawn>())
				{
					string text;
					Action meleeAttackAction = FloatMenuUtility.GetMeleeAttackAction(pawn2, target, out text, false);
					if (meleeAttackAction != null)
					{
						meleeAttackAction();
					}
					else if (!text.NullOrEmpty())
					{
						Messages.Message(text, target.Thing, MessageTypeDefOf.RejectInput, false);
					}
				}
			};
			return command_Target;
		}

		// Token: 0x060081D2 RID: 33234 RVA: 0x002E5340 File Offset: 0x002E3540
		private static bool AtLeastOneSelectedPlayerPawnHasRangedWeapon()
		{
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			for (int i = 0; i < selectedObjectsListForReading.Count; i++)
			{
				Pawn pawn = selectedObjectsListForReading[i] as Pawn;
				if (pawn != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn) && pawn.equipment != null && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060081D3 RID: 33235 RVA: 0x002E53B0 File Offset: 0x002E35B0
		private static bool AtLeastOneSelectedPlayerPawnHasNoWeapon()
		{
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			for (int i = 0; i < selectedObjectsListForReading.Count; i++)
			{
				Pawn pawn = selectedObjectsListForReading[i] as Pawn;
				if (pawn != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn) && (pawn.equipment == null || pawn.equipment.Primary == null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060081D4 RID: 33236 RVA: 0x002E540C File Offset: 0x002E360C
		private static bool AtLeastTwoSelectedPlayerPawnsHaveDifferentWeapons()
		{
			if (Find.Selector.NumSelected <= 1)
			{
				return false;
			}
			ThingDef thingDef = null;
			bool flag = false;
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			for (int i = 0; i < selectedObjectsListForReading.Count; i++)
			{
				Pawn pawn = selectedObjectsListForReading[i] as Pawn;
				if (pawn != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn))
				{
					ThingDef thingDef2;
					if (pawn.equipment == null || pawn.equipment.Primary == null)
					{
						thingDef2 = null;
					}
					else
					{
						thingDef2 = pawn.equipment.Primary.def;
					}
					if (!flag)
					{
						thingDef = thingDef2;
						flag = true;
					}
					else if (thingDef2 != thingDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060081D5 RID: 33237 RVA: 0x002E54A4 File Offset: 0x002E36A4
		private static bool AllSelectedPlayerPawnsCanTargetLocations()
		{
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn) && pawn.Drafted)
				{
					if (pawn.equipment.Primary == null || pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.IsMeleeAttack)
					{
						return false;
					}
					if (!pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.targetParams.canTargetLocations)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
