using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001951 RID: 6481
	[StaticConstructorOnStartup]
	public class Command_SetTargetFuelLevel : Command
	{
		// Token: 0x0600A2B4 RID: 41652 RVA: 0x003A1FE0 File Offset: 0x003A01E0
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			if (this.refuelables == null)
			{
				this.refuelables = new List<CompRefuelable>();
			}
			if (!this.refuelables.Contains(this.refuelable))
			{
				this.refuelables.Add(this.refuelable);
			}
			int num = int.MaxValue;
			for (int i = 0; i < this.refuelables.Count; i++)
			{
				if ((int)this.refuelables[i].Props.fuelCapacity < num)
				{
					num = (int)this.refuelables[i].Props.fuelCapacity;
				}
			}
			int startingValue = num / 2;
			for (int j = 0; j < this.refuelables.Count; j++)
			{
				if ((int)this.refuelables[j].TargetFuelLevel <= num)
				{
					startingValue = (int)this.refuelables[j].TargetFuelLevel;
					break;
				}
			}
			Func<int, string> textGetter;
			if (this.refuelable.parent.def.building.hasFuelingPort)
			{
				textGetter = delegate(int x)
				{
					CompLaunchable compLaunchable = FuelingPortUtility.LaunchableAt(FuelingPortUtility.GetFuelingPortCell(this.refuelable.parent.Position, this.refuelable.parent.Rotation), this.refuelable.parent.Map);
					if (compLaunchable == null)
					{
						return "SetTargetFuelLevel".Translate(x);
					}
					int value = compLaunchable.MaxLaunchDistanceAtFuelLevel((float)x);
					return "SetPodLauncherTargetFuelLevel".Translate(x, value);
				};
			}
			else
			{
				textGetter = ((int x) => "SetTargetFuelLevel".Translate(x));
			}
			Dialog_Slider dialog_Slider = new Dialog_Slider(textGetter, 0, num, delegate(int value)
			{
				for (int k = 0; k < this.refuelables.Count; k++)
				{
					this.refuelables[k].TargetFuelLevel = (float)value;
				}
			}, startingValue, 1f);
			if (this.refuelable.parent.def.building.hasFuelingPort)
			{
				dialog_Slider.extraBottomSpace = Text.LineHeight + 4f;
			}
			Find.WindowStack.Add(dialog_Slider);
		}

		// Token: 0x0600A2B5 RID: 41653 RVA: 0x003A2167 File Offset: 0x003A0367
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			if (this.refuelables == null)
			{
				this.refuelables = new List<CompRefuelable>();
			}
			this.refuelables.Add(((Command_SetTargetFuelLevel)other).refuelable);
			return false;
		}

		// Token: 0x0400602C RID: 24620
		public CompRefuelable refuelable;

		// Token: 0x0400602D RID: 24621
		private List<CompRefuelable> refuelables;
	}
}
