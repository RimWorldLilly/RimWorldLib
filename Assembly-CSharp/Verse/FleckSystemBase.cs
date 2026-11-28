using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	// Token: 0x02000266 RID: 614
	public class FleckSystemBase<TFleck> : FleckSystem where TFleck : struct, IFleck
	{
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x00066260 File Offset: 0x00064460
		protected virtual bool ParallelizedDrawing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00066263 File Offset: 0x00064463
		public FleckSystemBase(FleckManager parent) : base(parent)
		{
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x000662A4 File Offset: 0x000644A4
		public override void Update(float deltaTime)
		{
			try
			{
				for (int i = this.dataRealtime.Count - 1; i >= 0; i--)
				{
					TFleck value = this.dataRealtime[i];
					if (value.TimeInterval(deltaTime, this.parent.parent))
					{
						this.tmpRemoveIndices.Add(i);
					}
					else
					{
						this.dataRealtime[i] = value;
					}
				}
				this.dataRealtime.RemoveBatchUnordered(this.tmpRemoveIndices);
			}
			finally
			{
				this.tmpRemoveIndices.Clear();
			}
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0006633C File Offset: 0x0006453C
		public override void Tick()
		{
			try
			{
				for (int i = this.dataGametime.Count - 1; i >= 0; i--)
				{
					TFleck value = this.dataGametime[i];
					if (value.TimeInterval(0.016666668f, this.parent.parent))
					{
						this.tmpRemoveIndices.Add(i);
					}
					else
					{
						this.dataGametime[i] = value;
					}
				}
				this.dataGametime.RemoveBatchUnordered(this.tmpRemoveIndices);
			}
			finally
			{
				this.tmpRemoveIndices.Clear();
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x000663D8 File Offset: 0x000645D8
		private static void DrawParallel(object state)
		{
			if (WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				return;
			}
			FleckParallelizationInfo fleckParallelizationInfo = (FleckParallelizationInfo)state;
			try
			{
				List<FleckThrown> list = (List<FleckThrown>)fleckParallelizationInfo.data;
				for (int i = fleckParallelizationInfo.startIndex; i < fleckParallelizationInfo.endIndex; i++)
				{
					list[i].Draw(fleckParallelizationInfo.drawBatch);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			finally
			{
				fleckParallelizationInfo.doneEvent.Set();
			}
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00066464 File Offset: 0x00064664
		public override void ForceDraw(DrawBatch drawBatch)
		{
			FleckSystemBase<TFleck>.<>c__DisplayClass12_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.drawBatch = drawBatch;
			foreach (FleckDef fleckDef in this.handledDefs)
			{
				if (fleckDef.graphicData != null)
				{
					fleckDef.graphicData.ExplicitlyInitCachedGraphic();
				}
				if (fleckDef.randomGraphics != null)
				{
					foreach (GraphicData graphicData in fleckDef.randomGraphics)
					{
						graphicData.ExplicitlyInitCachedGraphic();
					}
				}
			}
			if (this.ParallelizedDrawing)
			{
				if (this.CachedDrawParallelWaitCallback == null)
				{
					this.CachedDrawParallelWaitCallback = new WaitCallback(FleckSystemBase<TFleck>.DrawParallel);
				}
				FleckSystemBase<TFleck>.<>c__DisplayClass12_1 CS$<>8__locals2;
				CS$<>8__locals2.parallelizationDegree = Environment.ProcessorCount;
				this.<ForceDraw>g__Process|12_1(this.dataRealtime, ref CS$<>8__locals1, ref CS$<>8__locals2);
				this.<ForceDraw>g__Process|12_1(this.dataGametime, ref CS$<>8__locals1, ref CS$<>8__locals2);
				return;
			}
			this.<ForceDraw>g__Process|12_0(this.dataRealtime, ref CS$<>8__locals1);
			this.<ForceDraw>g__Process|12_0(this.dataGametime, ref CS$<>8__locals1);
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00066588 File Offset: 0x00064788
		public override void CreateFleck(FleckCreationData creationData)
		{
			TFleck item = Activator.CreateInstance<TFleck>();
			item.Setup(creationData);
			if (creationData.def.realTime)
			{
				this.dataRealtime.Add(item);
				return;
			}
			this.dataGametime.Add(item);
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x000665D0 File Offset: 0x000647D0
		public override void MergeWith(FleckSystem system)
		{
			FleckSystemBase<TFleck> fleckSystemBase = system as FleckSystemBase<TFleck>;
			if (fleckSystemBase != null)
			{
				this.handledDefs.AddRangeUnique(fleckSystemBase.handledDefs);
				this.dataRealtime.AddRange(fleckSystemBase.dataRealtime);
				this.dataGametime.AddRange(fleckSystemBase.dataGametime);
				return;
			}
			throw new NotImplementedException("FleckSystemBase does not yet support merging with " + system.GetType().Name);
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00066635 File Offset: 0x00064835
		public override IEnumerable<IFleck> EnumerateFlecks()
		{
			FleckSystemBase<TFleck>.<EnumerateFlecks>d__15 <EnumerateFlecks>d__ = new FleckSystemBase<TFleck>.<EnumerateFlecks>d__15(-2);
			<EnumerateFlecks>d__.<>4__this = this;
			return <EnumerateFlecks>d__;
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x00066648 File Offset: 0x00064848
		public override void RemoveAllFlecks(Predicate<IFleck> shouldRemove)
		{
			for (int i = this.dataGametime.Count - 1; i >= 0; i--)
			{
				if (shouldRemove(this.dataGametime[i]))
				{
					this.dataGametime.RemoveAt(i);
				}
			}
			for (int j = this.dataRealtime.Count - 1; j >= 0; j--)
			{
				if (shouldRemove(this.dataRealtime[j]))
				{
					this.dataRealtime.RemoveAt(j);
				}
			}
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x000666CF File Offset: 0x000648CF
		public override void ExposeData()
		{
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x000666D4 File Offset: 0x000648D4
		[CompilerGenerated]
		private void <ForceDraw>g__Process|12_1(List<TFleck> data, ref FleckSystemBase<TFleck>.<>c__DisplayClass12_0 A_2, ref FleckSystemBase<TFleck>.<>c__DisplayClass12_1 A_3)
		{
			if (data.Count > 0)
			{
				try
				{
					this.tmpParallelizationSlices.Clear();
					GenThreading.SliceWorkNoAlloc(0, data.Count, A_3.parallelizationDegree, this.tmpParallelizationSlices);
					foreach (GenThreading.Slice slice in this.tmpParallelizationSlices)
					{
						FleckParallelizationInfo parallelizationInfo = FleckUtility.GetParallelizationInfo();
						parallelizationInfo.startIndex = slice.fromInclusive;
						parallelizationInfo.endIndex = slice.toExclusive;
						parallelizationInfo.data = data;
						ThreadPool.QueueUserWorkItem(this.CachedDrawParallelWaitCallback, parallelizationInfo);
						this.tmpParallelizationInfo.Add(parallelizationInfo);
					}
					foreach (FleckParallelizationInfo fleckParallelizationInfo in this.tmpParallelizationInfo)
					{
						fleckParallelizationInfo.doneEvent.WaitOne();
						A_2.drawBatch.MergeWith(fleckParallelizationInfo.drawBatch);
					}
				}
				finally
				{
					foreach (FleckParallelizationInfo info in this.tmpParallelizationInfo)
					{
						FleckUtility.ReturnParallelizationInfo(info);
					}
					this.tmpParallelizationInfo.Clear();
				}
			}
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00066840 File Offset: 0x00064A40
		[CompilerGenerated]
		private void <ForceDraw>g__Process|12_0(List<TFleck> data, ref FleckSystemBase<TFleck>.<>c__DisplayClass12_0 A_2)
		{
			for (int i = data.Count - 1; i >= 0; i--)
			{
				TFleck tfleck = data[i];
				tfleck.Draw(A_2.drawBatch);
			}
		}

		// Token: 0x04001036 RID: 4150
		private List<TFleck> dataRealtime = new List<TFleck>();

		// Token: 0x04001037 RID: 4151
		private List<TFleck> dataGametime = new List<TFleck>();

		// Token: 0x04001038 RID: 4152
		private List<GenThreading.Slice> tmpParallelizationSlices = new List<GenThreading.Slice>();

		// Token: 0x04001039 RID: 4153
		private List<FleckParallelizationInfo> tmpParallelizationInfo = new List<FleckParallelizationInfo>();

		// Token: 0x0400103A RID: 4154
		private List<int> tmpRemoveIndices = new List<int>();

		// Token: 0x0400103B RID: 4155
		private WaitCallback CachedDrawParallelWaitCallback;
	}
}
