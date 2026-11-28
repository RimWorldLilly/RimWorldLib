using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000952 RID: 2386
	public class Transition
	{
		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06004939 RID: 18745 RVA: 0x001C99F2 File Offset: 0x001C7BF2
		public Map Map
		{
			get
			{
				return this.target.Map;
			}
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x001C9A00 File Offset: 0x001C7C00
		public Transition(LordToil firstSource, LordToil target, bool canMoveToSameState = false, bool updateDutiesIfMovedToSameState = true)
		{
			this.canMoveToSameState = canMoveToSameState;
			this.updateDutiesIfMovedToSameState = updateDutiesIfMovedToSameState;
			this.target = target;
			this.sources = new List<LordToil>();
			this.AddSource(firstSource);
		}

		// Token: 0x0600493B RID: 18747 RVA: 0x001C9A64 File Offset: 0x001C7C64
		public void AddSource(LordToil source)
		{
			if (this.sources.Contains(source))
			{
				Log.Error("Double-added source to Transition: " + ((source != null) ? source.ToString() : null));
				return;
			}
			if (!this.canMoveToSameState && this.target == source)
			{
				Log.Error("Transition !canMoveToSameState and target is source: " + ((source != null) ? source.ToString() : null));
			}
			this.sources.Add(source);
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x001C9AD8 File Offset: 0x001C7CD8
		public void AddSources(IEnumerable<LordToil> sources)
		{
			foreach (LordToil source in sources)
			{
				this.AddSource(source);
			}
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x001C9B20 File Offset: 0x001C7D20
		public void AddSources(params LordToil[] sources)
		{
			for (int i = 0; i < sources.Length; i++)
			{
				this.AddSource(sources[i]);
			}
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x001C9B44 File Offset: 0x001C7D44
		public void AddTrigger(Trigger trigger)
		{
			this.triggers.Add(trigger);
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x001C9B52 File Offset: 0x001C7D52
		public void AddPreAction(TransitionAction action)
		{
			this.preActions.Add(action);
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x001C9B60 File Offset: 0x001C7D60
		public void AddPostAction(TransitionAction action)
		{
			this.postActions.Add(action);
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x001C9B70 File Offset: 0x001C7D70
		public void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				this.triggers[i].SourceToilBecameActive(transition, previousToil);
			}
		}

		// Token: 0x06004942 RID: 18754 RVA: 0x001C9BA8 File Offset: 0x001C7DA8
		public bool CheckSignal(Lord lord, TriggerSignal signal)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				if (this.triggers[i].ActivateOn(lord, signal))
				{
					if (this.triggers[i].filters != null)
					{
						bool flag = true;
						for (int j = 0; j < this.triggers[i].filters.Count; j++)
						{
							if (!this.triggers[i].filters[j].AllowActivation(lord, signal))
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							goto IL_119;
						}
					}
					if (DebugViewSettings.logLordToilTransitions)
					{
						string[] array = new string[8];
						array[0] = "Transitioning ";
						int num = 1;
						List<LordToil> list = this.sources;
						array[num] = ((list != null) ? list.ToString() : null);
						array[2] = " to ";
						int num2 = 3;
						LordToil lordToil = this.target;
						array[num2] = ((lordToil != null) ? lordToil.ToString() : null);
						array[4] = " by trigger ";
						int num3 = 5;
						Trigger trigger = this.triggers[i];
						array[num3] = ((trigger != null) ? trigger.ToString() : null);
						array[6] = " on signal ";
						int num4 = 7;
						TriggerSignal triggerSignal = signal;
						array[num4] = triggerSignal.ToString();
						Log.Message(string.Concat(array));
					}
					this.Execute(lord);
					return true;
				}
				IL_119:;
			}
			return false;
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x001C9CE4 File Offset: 0x001C7EE4
		public void Execute(Lord lord)
		{
			if (!this.canMoveToSameState && this.target == lord.CurLordToil)
			{
				return;
			}
			for (int i = 0; i < this.preActions.Count; i++)
			{
				try
				{
					this.preActions[i].DoAction(this);
				}
				catch (Exception ex)
				{
					string str = "Error in lord's preAction: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			if (this.target != lord.CurLordToil || this.updateDutiesIfMovedToSameState)
			{
				lord.GotoToil(this.target);
			}
			for (int j = 0; j < this.postActions.Count; j++)
			{
				try
				{
					this.postActions[j].DoAction(this);
				}
				catch (Exception ex3)
				{
					string str2 = "Error in lord's postAction: ";
					Exception ex4 = ex3;
					Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x001C9DDC File Offset: 0x001C7FDC
		public override string ToString()
		{
			string text = this.sources.NullOrEmpty<LordToil>() ? "null" : this.sources[0].ToString();
			int num = (this.sources == null) ? 0 : this.sources.Count;
			string text2 = (this.target == null) ? "null" : this.target.ToString();
			return string.Concat(new string[]
			{
				text,
				"(",
				num.ToString(),
				")->",
				text2
			});
		}

		// Token: 0x04002EFA RID: 12026
		public List<LordToil> sources;

		// Token: 0x04002EFB RID: 12027
		public LordToil target;

		// Token: 0x04002EFC RID: 12028
		public List<Trigger> triggers = new List<Trigger>();

		// Token: 0x04002EFD RID: 12029
		public List<TransitionAction> preActions = new List<TransitionAction>();

		// Token: 0x04002EFE RID: 12030
		public List<TransitionAction> postActions = new List<TransitionAction>();

		// Token: 0x04002EFF RID: 12031
		public bool canMoveToSameState;

		// Token: 0x04002F00 RID: 12032
		public bool updateDutiesIfMovedToSameState = true;
	}
}
