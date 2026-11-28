using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000261 RID: 609
	public sealed class FleckManager : IFleckCreator, IExposable
	{
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060011B8 RID: 4536 RVA: 0x00065DC8 File Offset: 0x00063FC8
		public IEnumerable<FleckSystem> Systems
		{
			get
			{
				return this.systems.Values;
			}
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00065DD8 File Offset: 0x00063FD8
		public FleckManager()
		{
			foreach (FleckDef fleckDef in DefDatabase<FleckDef>.AllDefsListForReading)
			{
				FleckSystem fleckSystem;
				if (!this.systems.TryGetValue(fleckDef.fleckSystemClass, out fleckSystem))
				{
					fleckSystem = this.CreateFleckSystemFor(fleckDef);
					this.systems.Add(fleckDef.fleckSystemClass, fleckSystem);
				}
				fleckSystem.handledDefs.Add(fleckDef);
			}
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00065E7C File Offset: 0x0006407C
		public FleckManager(Map parent) : this()
		{
			this.parent = parent;
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00065E8B File Offset: 0x0006408B
		public FleckSystem CreateFleckSystemFor(FleckDef def)
		{
			return (FleckSystem)Activator.CreateInstance(def.fleckSystemClass, new object[]
			{
				this
			});
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00065EA8 File Offset: 0x000640A8
		public void CreateFleck(FleckCreationData fleckData)
		{
			FleckSystem fleckSystem;
			if (!this.systems.TryGetValue(fleckData.def.fleckSystemClass, out fleckSystem))
			{
				string str = "No system to handle MoteDef ";
				FleckDef def = fleckData.def;
				throw new Exception(str + ((def != null) ? def.ToString() : null) + " found!?");
			}
			fleckData.spawnPosition.y = fleckData.def.altitudeLayer.AltitudeFor(fleckData.def.altitudeLayerIncOffset);
			fleckSystem.CreateFleck(fleckData);
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00065F24 File Offset: 0x00064124
		public void FleckManagerUpdate()
		{
			float deltaTime = Time.deltaTime;
			foreach (FleckSystem fleckSystem in this.systems.Values)
			{
				fleckSystem.Update(deltaTime);
			}
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00065F80 File Offset: 0x00064180
		public void FleckManagerTick()
		{
			foreach (FleckSystem fleckSystem in this.systems.Values)
			{
				fleckSystem.Tick();
			}
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x00065FD8 File Offset: 0x000641D8
		public void FleckManagerDraw()
		{
			try
			{
				foreach (FleckSystem fleckSystem in this.systems.Values)
				{
					fleckSystem.Draw(this.drawBatch);
				}
			}
			finally
			{
				this.drawBatch.Flush(true);
			}
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0006604C File Offset: 0x0006424C
		public void HandOverSystem(FleckSystem system)
		{
			Type type = system.GetType();
			FleckSystem fleckSystem;
			if (this.systems.TryGetValue(type, out fleckSystem))
			{
				fleckSystem.MergeWith(system);
				return;
			}
			this.systems.Add(type, system);
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00066088 File Offset: 0x00064288
		public void FleckManagerOnGUI()
		{
			foreach (FleckSystem fleckSystem in this.systems.Values)
			{
				fleckSystem.OnGUI();
			}
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x000660E0 File Offset: 0x000642E0
		public void ExposeData()
		{
		}

		// Token: 0x0400102D RID: 4141
		public readonly Map parent;

		// Token: 0x0400102E RID: 4142
		private Dictionary<Type, FleckSystem> systems = new Dictionary<Type, FleckSystem>();

		// Token: 0x0400102F RID: 4143
		private DrawBatch drawBatch = new DrawBatch();
	}
}
