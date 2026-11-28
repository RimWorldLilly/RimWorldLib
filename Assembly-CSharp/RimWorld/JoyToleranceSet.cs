using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001563 RID: 5475
	public class JoyToleranceSet : IExposable
	{
		// Token: 0x170016BA RID: 5818
		public float this[JoyKindDef d]
		{
			get
			{
				return this.tolerances[d];
			}
		}

		// Token: 0x06008641 RID: 34369 RVA: 0x00308B2E File Offset: 0x00306D2E
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<JoyKindDef, float>>(ref this.tolerances, "tolerances", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<JoyKindDef, bool>>(ref this.bored, "bored", Array.Empty<object>());
			if (this.bored == null)
			{
				this.bored = new DefMap<JoyKindDef, bool>();
			}
		}

		// Token: 0x06008642 RID: 34370 RVA: 0x00308B6D File Offset: 0x00306D6D
		public bool BoredOf(JoyKindDef def)
		{
			return this.bored[def];
		}

		// Token: 0x06008643 RID: 34371 RVA: 0x00308B7C File Offset: 0x00306D7C
		public void Notify_JoyGained(float amount, JoyKindDef joyKind)
		{
			float num = Mathf.Min(this.tolerances[joyKind] + amount * 0.65f, 1f);
			this.tolerances[joyKind] = num;
			if (num > 0.5f)
			{
				this.bored[joyKind] = true;
			}
		}

		// Token: 0x06008644 RID: 34372 RVA: 0x00308BCA File Offset: 0x00306DCA
		public float JoyFactorFromTolerance(JoyKindDef joyKind)
		{
			return 1f - this.tolerances[joyKind];
		}

		// Token: 0x06008645 RID: 34373 RVA: 0x00308BE0 File Offset: 0x00306DE0
		public void NeedInterval(Pawn pawn)
		{
			float num = ExpectationsUtility.CurrentExpectationFor(pawn).joyToleranceDropPerDay * 150f / 60000f;
			for (int i = 0; i < this.tolerances.Count; i++)
			{
				float num2 = this.tolerances[i];
				num2 -= num;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				this.tolerances[i] = num2;
				if (this.bored[i] && num2 < 0.3f)
				{
					this.bored[i] = false;
				}
			}
		}

		// Token: 0x06008646 RID: 34374 RVA: 0x00308C6C File Offset: 0x00306E6C
		public string TolerancesString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<JoyKindDef> allDefsListForReading = DefDatabase<JoyKindDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = allDefsListForReading[i];
				float num = this.tolerances[joyKindDef];
				if (num > 0.01f)
				{
					if (stringBuilder.Length == 0)
					{
						stringBuilder.AppendLine("JoyTolerances".Translate() + ":");
					}
					string text = "   " + joyKindDef.LabelCap + ": " + num.ToStringPercent();
					if (this.bored[joyKindDef])
					{
						text += " (" + "bored".Translate() + ")";
					}
					stringBuilder.AppendLine(text);
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06008647 RID: 34375 RVA: 0x00308D64 File Offset: 0x00306F64
		public bool BoredOfKinds(List<JoyKindDef> kindsAvailable)
		{
			for (int i = 0; i < kindsAvailable.Count; i++)
			{
				if (!this.bored[kindsAvailable[i]])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008648 RID: 34376 RVA: 0x00308D9C File Offset: 0x00306F9C
		public bool BoredOfAllAvailableJoyKinds(Pawn pawn)
		{
			List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(pawn.MapHeld);
			bool result = true;
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.bored[list[i]])
				{
					result = false;
					break;
				}
			}
			list.Clear();
			return result;
		}

		// Token: 0x040052A8 RID: 21160
		private DefMap<JoyKindDef, float> tolerances = new DefMap<JoyKindDef, float>();

		// Token: 0x040052A9 RID: 21161
		private DefMap<JoyKindDef, bool> bored = new DefMap<JoyKindDef, bool>();
	}
}
