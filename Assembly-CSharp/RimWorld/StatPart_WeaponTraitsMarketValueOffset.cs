using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FAF RID: 8111
	public class StatPart_WeaponTraitsMarketValueOffset : StatPart
	{
		// Token: 0x0600CDEB RID: 52715 RVA: 0x004BA450 File Offset: 0x004B8650
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				CompBladelinkWeapon compBladelinkWeapon = req.Thing.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
					if (!traitsListForReading.NullOrEmpty<WeaponTraitDef>())
					{
						for (int i = 0; i < traitsListForReading.Count; i++)
						{
							val += traitsListForReading[i].marketValueOffset;
						}
					}
				}
			}
		}

		// Token: 0x0600CDEC RID: 52716 RVA: 0x004BA4A8 File Offset: 0x004B86A8
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				CompBladelinkWeapon compBladelinkWeapon = req.Thing.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
					if (!traitsListForReading.NullOrEmpty<WeaponTraitDef>())
					{
						for (int i = 0; i < traitsListForReading.Count; i++)
						{
							if (traitsListForReading[i].marketValueOffset != 0f)
							{
								stringBuilder.AppendLine(traitsListForReading[i].LabelCap + ": " + traitsListForReading[i].marketValueOffset.ToStringByStyle(ToStringStyle.Money, ToStringNumberSense.Offset));
							}
						}
					}
					return stringBuilder.ToString();
				}
			}
			return null;
		}
	}
}
