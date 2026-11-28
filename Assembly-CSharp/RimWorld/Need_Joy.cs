using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001577 RID: 5495
	public class Need_Joy : Need
	{
		// Token: 0x170016F4 RID: 5876
		// (get) Token: 0x060086C0 RID: 34496 RVA: 0x0030B368 File Offset: 0x00309568
		public JoyCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return JoyCategory.Empty;
				}
				if (this.CurLevel < 0.15f)
				{
					return JoyCategory.VeryLow;
				}
				if (this.CurLevel < 0.3f)
				{
					return JoyCategory.Low;
				}
				if (this.CurLevel < 0.7f)
				{
					return JoyCategory.Satisfied;
				}
				if (this.CurLevel < 0.85f)
				{
					return JoyCategory.High;
				}
				return JoyCategory.Extreme;
			}
		}

		// Token: 0x170016F5 RID: 5877
		// (get) Token: 0x060086C1 RID: 34497 RVA: 0x0030B3C4 File Offset: 0x003095C4
		private float FallPerInterval
		{
			get
			{
				switch (this.CurCategory)
				{
				case JoyCategory.Empty:
					return 0.0015f;
				case JoyCategory.VeryLow:
					return 0.0006f;
				case JoyCategory.Low:
					return 0.00105f;
				case JoyCategory.Satisfied:
					return 0.0015f;
				case JoyCategory.High:
					return 0.0015f;
				case JoyCategory.Extreme:
					return 0.0015f;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		// Token: 0x170016F6 RID: 5878
		// (get) Token: 0x060086C2 RID: 34498 RVA: 0x0030B421 File Offset: 0x00309621
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				if (!this.GainingJoy)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x170016F7 RID: 5879
		// (get) Token: 0x060086C3 RID: 34499 RVA: 0x0030B438 File Offset: 0x00309638
		private bool GainingJoy
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastGainTick + 15;
			}
		}

		// Token: 0x060086C4 RID: 34500 RVA: 0x0030B450 File Offset: 0x00309650
		public Need_Joy(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.85f);
		}

		// Token: 0x060086C5 RID: 34501 RVA: 0x0030B4C5 File Offset: 0x003096C5
		public override void ExposeData()
		{
			base.ExposeData();
			this.tolerances.ExposeData();
		}

		// Token: 0x060086C6 RID: 34502 RVA: 0x0030B4D8 File Offset: 0x003096D8
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.5f, 0.6f);
		}

		// Token: 0x060086C7 RID: 34503 RVA: 0x0030B4F0 File Offset: 0x003096F0
		public void GainJoy(float amount, JoyKindDef joyKind)
		{
			if (amount <= 0f)
			{
				return;
			}
			amount *= this.tolerances.JoyFactorFromTolerance(joyKind);
			amount = Mathf.Min(amount, 1f - this.CurLevel);
			this.curLevelInt += amount;
			if (joyKind != null)
			{
				this.tolerances.Notify_JoyGained(amount, joyKind);
			}
			this.lastGainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060086C8 RID: 34504 RVA: 0x0030B558 File Offset: 0x00309758
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.tolerances.NeedInterval(this.pawn);
				if (!this.GainingJoy)
				{
					this.CurLevel -= this.FallPerInterval * (this.pawn.IsFormingCaravan() ? 0.5f : 1f) * this.pawn.GetStatValue(StatDefOf.JoyFallRateFactor, true, -1);
				}
			}
		}

		// Token: 0x060086C9 RID: 34505 RVA: 0x0030B5C8 File Offset: 0x003097C8
		public override string GetTipString()
		{
			TaggedString t = base.GetTipString();
			string text = this.tolerances.TolerancesString();
			if (!string.IsNullOrEmpty(text))
			{
				t += "\n\n" + text;
			}
			if (this.pawn.MapHeld != null)
			{
				ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(this.pawn);
				t += "\n\n" + "CurrentExpectationsAndRecreation".Translate(expectationDef.label, expectationDef.joyToleranceDropPerDay.ToStringPercent(), expectationDef.joyKindsNeeded);
				t += "\n\n" + JoyUtility.JoyKindsOnMapString(this.pawn.MapHeld);
			}
			else
			{
				Caravan caravan = this.pawn.GetCaravan();
				if (caravan != null)
				{
					float num = caravan.needs.GetCurrentJoyGainPerTick(this.pawn) * 2500f;
					if (num > 0f)
					{
						t += "\n\n" + "GainingJoyBecauseCaravanNotMoving".Translate() + ": +" + num.ToStringPercent() + "/" + "LetterHour".Translate();
					}
				}
			}
			return t.Resolve();
		}

		// Token: 0x04005327 RID: 21287
		public JoyToleranceSet tolerances = new JoyToleranceSet();

		// Token: 0x04005328 RID: 21288
		private int lastGainTick = -999;
	}
}
