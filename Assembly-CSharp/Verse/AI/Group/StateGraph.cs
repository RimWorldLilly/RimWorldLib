using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI.Group
{
	// Token: 0x02000951 RID: 2385
	public class StateGraph : IDisposable
	{
		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06004930 RID: 18736 RVA: 0x001C9685 File Offset: 0x001C7885
		// (set) Token: 0x06004931 RID: 18737 RVA: 0x001C9693 File Offset: 0x001C7893
		public LordToil StartingToil
		{
			get
			{
				return this.lordToils[0];
			}
			set
			{
				if (this.lordToils.Contains(value))
				{
					this.lordToils.Remove(value);
				}
				this.lordToils.Insert(0, value);
			}
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x001C96BD File Offset: 0x001C78BD
		public void AddToil(LordToil toil)
		{
			this.lordToils.Add(toil);
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x001C96CB File Offset: 0x001C78CB
		public void AddTransition(Transition transition, bool highPriority = false)
		{
			if (highPriority)
			{
				this.transitions.Insert(0, transition);
				return;
			}
			this.transitions.Add(transition);
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x001C96EC File Offset: 0x001C78EC
		public StateGraph AttachSubgraph(StateGraph subGraph)
		{
			for (int i = 0; i < subGraph.lordToils.Count; i++)
			{
				this.lordToils.Add(subGraph.lordToils[i]);
			}
			for (int j = 0; j < subGraph.transitions.Count; j++)
			{
				this.transitions.Add(subGraph.transitions[j]);
			}
			return subGraph;
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x001C9754 File Offset: 0x001C7954
		public void ErrorCheck()
		{
			if (this.lordToils.Count == 0)
			{
				Log.Error("Graph has 0 lord toils.");
			}
			using (IEnumerator<LordToil> enumerator = this.lordToils.Distinct<LordToil>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LordToil toil = enumerator.Current;
					int num = this.lordToils.Count((LordToil s) => s == toil);
					if (num != 1)
					{
						string[] array = new string[5];
						array[0] = "Graph has lord toil ";
						int num2 = 1;
						LordToil toil2 = toil;
						array[num2] = ((toil2 != null) ? toil2.ToString() : null);
						array[2] = " registered ";
						array[3] = num.ToString();
						array[4] = " times.";
						Log.Error(string.Concat(array));
					}
				}
			}
			using (List<Transition>.Enumerator enumerator2 = this.transitions.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Transition trans = enumerator2.Current;
					int num3 = this.transitions.Count((Transition t) => t == trans);
					if (num3 != 1)
					{
						string[] array2 = new string[5];
						array2[0] = "Graph has transition ";
						int num4 = 1;
						Transition trans2 = trans;
						array2[num4] = ((trans2 != null) ? trans2.ToString() : null);
						array2[2] = " registered ";
						array2[3] = num3.ToString();
						array2[4] = " times.";
						Log.Error(string.Concat(array2));
					}
				}
			}
			StateGraph.checkedToils = new HashSet<LordToil>();
			this.CheckForUnregisteredLinkedToilsRecursive(this.StartingToil);
			StateGraph.checkedToils = null;
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x001C98F4 File Offset: 0x001C7AF4
		private void CheckForUnregisteredLinkedToilsRecursive(LordToil toil)
		{
			if (!this.lordToils.Contains(toil))
			{
				Log.Error("Unregistered linked lord toil: " + ((toil != null) ? toil.ToString() : null));
			}
			StateGraph.checkedToils.Add(toil);
			for (int i = 0; i < this.transitions.Count; i++)
			{
				Transition transition = this.transitions[i];
				if (transition.sources.Contains(toil) && !StateGraph.checkedToils.Contains(toil))
				{
					this.CheckForUnregisteredLinkedToilsRecursive(transition.target);
				}
			}
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x001C9984 File Offset: 0x001C7B84
		public void Dispose()
		{
			foreach (LordToil lordToil in this.lordToils)
			{
				lordToil.Dispose();
			}
		}

		// Token: 0x04002EF7 RID: 12023
		public List<LordToil> lordToils = new List<LordToil>();

		// Token: 0x04002EF8 RID: 12024
		public List<Transition> transitions = new List<Transition>();

		// Token: 0x04002EF9 RID: 12025
		private static HashSet<LordToil> checkedToils;
	}
}
