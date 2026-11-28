using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000265 RID: 613
	public abstract class FleckSystem : IFleckCreator, IExposable, ILoadReferenceable
	{
		// Token: 0x060011CE RID: 4558 RVA: 0x000661A0 File Offset: 0x000643A0
		public FleckSystem(FleckManager parent)
		{
			this.parent = parent;
		}

		// Token: 0x060011CF RID: 4559
		public abstract void Update(float deltaTime);

		// Token: 0x060011D0 RID: 4560
		public abstract void Tick();

		// Token: 0x060011D1 RID: 4561
		public abstract void ForceDraw(DrawBatch drawBatch);

		// Token: 0x060011D2 RID: 4562 RVA: 0x000661BA File Offset: 0x000643BA
		public virtual void Draw(DrawBatch drawBatch)
		{
			if (WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				return;
			}
			this.ForceDraw(drawBatch);
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x000661CB File Offset: 0x000643CB
		public virtual void OnGUI()
		{
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x000661D0 File Offset: 0x000643D0
		public virtual void Prewarm(float prewarmSeconds, Action<float> onUpdate, Action onTick)
		{
			float num = 0f;
			float num2 = 0f;
			while (num < prewarmSeconds)
			{
				this.Update(0.06666667f);
				if (onUpdate != null)
				{
					onUpdate(0.06666667f);
				}
				while (num2 >= 0.016666668f)
				{
					this.Tick();
					if (onTick != null)
					{
						onTick();
					}
					num2 -= 0.016666668f;
				}
				num += 0.06666667f;
				num2 += 0.06666667f;
			}
		}

		// Token: 0x060011D5 RID: 4565
		public abstract void CreateFleck(FleckCreationData fleckData);

		// Token: 0x060011D6 RID: 4566
		public abstract void MergeWith(FleckSystem system);

		// Token: 0x060011D7 RID: 4567
		public abstract IEnumerable<IFleck> EnumerateFlecks();

		// Token: 0x060011D8 RID: 4568
		public abstract void RemoveAllFlecks(Predicate<IFleck> shouldRemove);

		// Token: 0x060011D9 RID: 4569
		public abstract void ExposeData();

		// Token: 0x060011DA RID: 4570 RVA: 0x00066239 File Offset: 0x00064439
		public string GetUniqueLoadID()
		{
			return this.parent.parent.GetUniqueLoadID() + "_FleckSystem_" + base.GetType().FullName;
		}

		// Token: 0x04001034 RID: 4148
		public List<FleckDef> handledDefs = new List<FleckDef>();

		// Token: 0x04001035 RID: 4149
		public FleckManager parent;
	}
}
