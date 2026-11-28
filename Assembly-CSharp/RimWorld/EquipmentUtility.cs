using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F14 RID: 7956
	public static class EquipmentUtility
	{
		// Token: 0x0600CAD1 RID: 51921 RVA: 0x004A6A1C File Offset: 0x004A4C1C
		public static bool CanEquip(Thing thing, Pawn pawn)
		{
			string text;
			return EquipmentUtility.CanEquip(thing, pawn, out text, true);
		}

		// Token: 0x0600CAD2 RID: 51922 RVA: 0x004A6A34 File Offset: 0x004A4C34
		public static bool CanEquip(Thing thing, Pawn pawn, out string cantReason, bool checkBonded = true)
		{
			cantReason = null;
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon != null && compBladelinkWeapon.Biocodable && compBladelinkWeapon.CodedPawn != null && compBladelinkWeapon.CodedPawn != pawn)
			{
				cantReason = "BladelinkBondedToSomeoneElse".Translate();
				return false;
			}
			if (CompBiocodable.IsBiocoded(thing) && !CompBiocodable.IsBiocodedFor(thing, pawn))
			{
				cantReason = "BiocodedCodedForSomeoneElse".Translate();
				return false;
			}
			if (checkBonded && EquipmentUtility.AlreadyBondedToWeapon(thing, pawn))
			{
				cantReason = "BladelinkAlreadyBondedMessage".Translate(pawn.Named("PAWN"), pawn.equipment.bondedWeapon.Named("BONDEDWEAPON"));
				return false;
			}
			if (EquipmentUtility.RolePreventsFromUsing(pawn, thing, out cantReason))
			{
				return false;
			}
			if (thing.def.IsApparel && !thing.def.apparel.developmentalStageFilter.Has(pawn.DevelopmentalStage))
			{
				cantReason = "WrongDevelopmentalStageForClothing".Translate(pawn.DevelopmentalStage.ToString().Translate(), Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(thing.def.apparel.developmentalStageFilter.ToCommaListOr(), false, false));
				return false;
			}
			return true;
		}

		// Token: 0x0600CAD3 RID: 51923 RVA: 0x004A6B6C File Offset: 0x004A4D6C
		public static bool AlreadyBondedToWeapon(Thing thing, Pawn pawn)
		{
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			return compBladelinkWeapon != null && compBladelinkWeapon.Biocodable && pawn.equipment.bondedWeapon != null && pawn.equipment.bondedWeapon != thing;
		}

		// Token: 0x0600CAD4 RID: 51924 RVA: 0x004A6BB0 File Offset: 0x004A4DB0
		public static string GetPersonaWeaponConfirmationText(Thing item, Pawn p)
		{
			CompBladelinkWeapon compBladelinkWeapon = item.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon != null && compBladelinkWeapon.Biocodable && compBladelinkWeapon.CodedPawn != p)
			{
				TaggedString taggedString = "BladelinkEquipWarning".Translate();
				List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
				if (!traitsListForReading.NullOrEmpty<WeaponTraitDef>())
				{
					taggedString += "\n\n" + "BladelinkEquipWarningTraits".Translate() + ":";
					for (int i = 0; i < traitsListForReading.Count; i++)
					{
						taggedString += "\n\n" + traitsListForReading[i].LabelCap + ": " + traitsListForReading[i].description;
					}
				}
				taggedString += "\n\n" + "RoyalWeaponEquipConfirmation".Translate();
				return taggedString;
			}
			return null;
		}

		// Token: 0x0600CAD5 RID: 51925 RVA: 0x004A6C8C File Offset: 0x004A4E8C
		public static bool IsBondedTo(Thing thing, Pawn pawn)
		{
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			return compBladelinkWeapon != null && compBladelinkWeapon.CodedPawn == pawn;
		}

		// Token: 0x0600CAD6 RID: 51926 RVA: 0x004A6CB0 File Offset: 0x004A4EB0
		public static bool QuestLodgerCanEquip(Thing thing, Pawn pawn)
		{
			return (pawn.equipment.Primary == null || EquipmentUtility.QuestLodgerCanUnequip(pawn.equipment.Primary, pawn)) && (CompBiocodable.IsBiocodedFor(thing, pawn) || EquipmentUtility.AlreadyBondedToWeapon(thing, pawn) || thing.def.IsWeapon);
		}

		// Token: 0x0600CAD7 RID: 51927 RVA: 0x004A6D00 File Offset: 0x004A4F00
		public static bool RolePreventsFromUsing(Pawn pawn, Thing thing, out string reason)
		{
			if (ModsConfig.IdeologyActive && pawn.Ideo != null)
			{
				Precept_Role role = pawn.Ideo.GetRole(pawn);
				if (role != null && !role.CanEquip(pawn, thing, out reason))
				{
					return true;
				}
			}
			reason = null;
			return false;
		}

		// Token: 0x0600CAD8 RID: 51928 RVA: 0x004A6D3D File Offset: 0x004A4F3D
		public static bool QuestLodgerCanUnequip(Thing thing, Pawn pawn)
		{
			return !CompBiocodable.IsBiocodedFor(thing, pawn) && !EquipmentUtility.IsBondedTo(thing, pawn);
		}

		// Token: 0x0600CAD9 RID: 51929 RVA: 0x004A6D58 File Offset: 0x004A4F58
		public static Verb_LaunchProjectile GetRecoilVerb(List<Verb> allWeaponVerbs)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = null;
			foreach (Verb verb in allWeaponVerbs)
			{
				Verb_LaunchProjectile verb_LaunchProjectile2 = verb as Verb_LaunchProjectile;
				if (verb_LaunchProjectile2 != null && (verb_LaunchProjectile == null || verb_LaunchProjectile.LastShotTick < verb_LaunchProjectile2.LastShotTick))
				{
					verb_LaunchProjectile = verb_LaunchProjectile2;
				}
			}
			return verb_LaunchProjectile;
		}

		// Token: 0x0600CADA RID: 51930 RVA: 0x004A6DC0 File Offset: 0x004A4FC0
		public static void Recoil(ThingDef weaponDef, Verb_LaunchProjectile shootVerb, out Vector3 drawOffset, out float angleOffset, float aimAngle)
		{
			drawOffset = Vector3.zero;
			angleOffset = 0f;
			if (weaponDef.recoilPower > 0f && shootVerb != null)
			{
				Rand.PushState(shootVerb.LastShotTick);
				try
				{
					int num = Find.TickManager.TicksGame - shootVerb.LastShotTick;
					if ((float)num < weaponDef.recoilRelaxation)
					{
						float num2 = Mathf.Clamp01((float)num / weaponDef.recoilRelaxation);
						float num3 = Mathf.Lerp(weaponDef.recoilPower, 0f, num2);
						drawOffset = new Vector3((float)Rand.Sign * EquipmentUtility.RecoilCurveAxisX.Evaluate(num2), 0f, -EquipmentUtility.RecoilCurveAxisY.Evaluate(num2)) * num3;
						angleOffset = (float)Rand.Sign * EquipmentUtility.RecoilCurveRotation.Evaluate(num2) * num3;
						aimAngle += angleOffset;
						drawOffset = drawOffset.RotatedBy(aimAngle);
					}
				}
				finally
				{
					Rand.PopState();
				}
			}
		}

		// Token: 0x0600CADB RID: 51931 RVA: 0x004A6EBC File Offset: 0x004A50BC
		public static bool TryGenerateWeaponByTag(string weaponTag, out Thing weapon, float marketValue = 99999f)
		{
			EquipmentUtility.workingWeapons.Clear();
			weapon = null;
			foreach (ThingStuffPair thingStuffPair in PawnWeaponGenerator.AllWeaponPairs)
			{
				if (thingStuffPair.Price <= marketValue && thingStuffPair.thing.weaponTags != null && thingStuffPair.thing.weaponTags.Contains(weaponTag))
				{
					EquipmentUtility.workingWeapons.Add(thingStuffPair);
				}
			}
			if (EquipmentUtility.workingWeapons.Count == 0)
			{
				return false;
			}
			ThingStuffPair thingStuffPair2;
			if (!EquipmentUtility.workingWeapons.TryRandomElementByWeight((ThingStuffPair w) => w.Commonality * w.Price, out thingStuffPair2))
			{
				return false;
			}
			weapon = (ThingWithComps)ThingMaker.MakeThing(thingStuffPair2.thing, thingStuffPair2.stuff);
			return true;
		}

		// Token: 0x0400816F RID: 33135
		private static readonly SimpleCurve RecoilCurveAxisX = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0.02f),
				true
			},
			{
				new CurvePoint(2f, 0.03f),
				true
			}
		};

		// Token: 0x04008170 RID: 33136
		private static readonly SimpleCurve RecoilCurveAxisY = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0.05f),
				true
			},
			{
				new CurvePoint(2f, 0.075f),
				true
			}
		};

		// Token: 0x04008171 RID: 33137
		private static readonly SimpleCurve RecoilCurveRotation = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 3f),
				true
			},
			{
				new CurvePoint(2f, 4f),
				true
			}
		};

		// Token: 0x04008172 RID: 33138
		private static List<ThingStuffPair> workingWeapons = new List<ThingStuffPair>();
	}
}
