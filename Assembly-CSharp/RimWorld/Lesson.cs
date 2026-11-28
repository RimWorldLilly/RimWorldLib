using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E18 RID: 7704
	public abstract class Lesson : IExposable
	{
		// Token: 0x170024F2 RID: 9458
		// (get) Token: 0x0600C820 RID: 51232 RVA: 0x00496574 File Offset: 0x00494774
		protected float AgeSeconds
		{
			get
			{
				if (this.startRealTime < 0f)
				{
					this.startRealTime = Time.realtimeSinceStartup;
				}
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x170024F3 RID: 9459
		// (get) Token: 0x0600C821 RID: 51233 RVA: 0x0049659A File Offset: 0x0049479A
		public virtual ConceptDef Concept
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170024F4 RID: 9460
		// (get) Token: 0x0600C822 RID: 51234 RVA: 0x0049659D File Offset: 0x0049479D
		public virtual InstructionDef Instruction
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170024F5 RID: 9461
		// (get) Token: 0x0600C823 RID: 51235 RVA: 0x004965A0 File Offset: 0x004947A0
		public virtual float MessagesYOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600C824 RID: 51236 RVA: 0x004965A7 File Offset: 0x004947A7
		public virtual void ExposeData()
		{
		}

		// Token: 0x0600C825 RID: 51237 RVA: 0x004965A9 File Offset: 0x004947A9
		public virtual void OnActivated()
		{
			this.startRealTime = Time.realtimeSinceStartup;
		}

		// Token: 0x0600C826 RID: 51238 RVA: 0x004965B6 File Offset: 0x004947B6
		public virtual void PostDeactivated()
		{
		}

		// Token: 0x0600C827 RID: 51239
		public abstract void LessonOnGUI();

		// Token: 0x0600C828 RID: 51240 RVA: 0x004965B8 File Offset: 0x004947B8
		public virtual void LessonUpdate()
		{
		}

		// Token: 0x0600C829 RID: 51241 RVA: 0x004965BA File Offset: 0x004947BA
		public virtual void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
		}

		// Token: 0x0600C82A RID: 51242 RVA: 0x004965BC File Offset: 0x004947BC
		public virtual void Notify_Event(EventPack ep)
		{
		}

		// Token: 0x0600C82B RID: 51243 RVA: 0x004965BE File Offset: 0x004947BE
		public virtual AcceptanceReport AllowAction(EventPack ep)
		{
			return true;
		}

		// Token: 0x170024F6 RID: 9462
		// (get) Token: 0x0600C82C RID: 51244 RVA: 0x004965C6 File Offset: 0x004947C6
		public virtual string DefaultRejectInputMessage
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040071E4 RID: 29156
		public float startRealTime = -999f;

		// Token: 0x040071E5 RID: 29157
		public const float KnowledgeForAutoVanish = 0.2f;
	}
}
