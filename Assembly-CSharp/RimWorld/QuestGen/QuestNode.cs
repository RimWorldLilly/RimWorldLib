using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002246 RID: 8774
	public abstract class QuestNode
	{
		// Token: 0x0600D906 RID: 55558 RVA: 0x00506404 File Offset: 0x00504604
		public QuestNode()
		{
			this.myTypeShort = base.GetType().Name;
			if (this.myTypeShort.StartsWith("QuestNode_"))
			{
				this.myTypeShort = this.myTypeShort.Substring("QuestNode_".Length);
			}
		}

		// Token: 0x0600D907 RID: 55559 RVA: 0x00506458 File Offset: 0x00504658
		public float SelectionWeight(Slate slate)
		{
			return this.selectionWeight.GetValue(slate).GetValueOrDefault(1f);
		}

		// Token: 0x0600D908 RID: 55560 RVA: 0x00506480 File Offset: 0x00504680
		public void Run()
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start(this.ToString());
			}
			try
			{
				this.RunInt();
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception running ";
				array[1] = base.GetType().Name;
				array[2] = ": ";
				int num = 3;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				array[4] = "\n\nSlate vars:\n";
				array[5] = QuestGen.slate.ToString();
				Log.Error(string.Concat(array));
			}
			if (DeepProfiler.enabled)
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600D909 RID: 55561 RVA: 0x00506520 File Offset: 0x00504720
		public bool TestRun(Slate slate)
		{
			bool result;
			try
			{
				Action<QuestNode, Slate> action;
				if (slate.TryGet<Action<QuestNode, Slate>>("testRunCallback", out action, false) && action != null)
				{
					action(this, slate);
				}
				result = this.TestRunInt(slate);
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Exception test running ");
				defaultInterpolatedStringHandler.AppendFormatted(base.GetType().Name);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				defaultInterpolatedStringHandler.AppendLiteral("\n\nSlate vars:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Slate>(slate);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				result = false;
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x0600D90A RID: 55562
		protected abstract void RunInt();

		// Token: 0x0600D90B RID: 55563
		protected abstract bool TestRunInt(Slate slate);

		// Token: 0x0600D90C RID: 55564 RVA: 0x005065D0 File Offset: 0x005047D0
		public override string ToString()
		{
			return base.GetType().Name;
		}

		// Token: 0x04008AB5 RID: 35509
		[Unsaved(false)]
		[TranslationHandle]
		public string myTypeShort;

		// Token: 0x04008AB6 RID: 35510
		public SlateRef<float?> selectionWeight;
	}
}
