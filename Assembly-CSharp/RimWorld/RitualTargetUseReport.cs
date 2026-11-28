using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001693 RID: 5779
	public struct RitualTargetUseReport
	{
		// Token: 0x17001905 RID: 6405
		// (get) Token: 0x060090DC RID: 37084 RVA: 0x00352405 File Offset: 0x00350605
		public bool ShouldShowGizmo
		{
			get
			{
				return this.canUse || !this.failReason.NullOrEmpty();
			}
		}

		// Token: 0x060090DD RID: 37085 RVA: 0x00352420 File Offset: 0x00350620
		public static implicit operator RitualTargetUseReport(bool canUse)
		{
			return new RitualTargetUseReport
			{
				canUse = canUse,
				failReason = null
			};
		}

		// Token: 0x060090DE RID: 37086 RVA: 0x00352448 File Offset: 0x00350648
		public static implicit operator RitualTargetUseReport(string failReason)
		{
			return new RitualTargetUseReport
			{
				canUse = false,
				failReason = failReason
			};
		}

		// Token: 0x060090DF RID: 37087 RVA: 0x00352470 File Offset: 0x00350670
		public static implicit operator RitualTargetUseReport(TaggedString failReason)
		{
			return new RitualTargetUseReport
			{
				canUse = false,
				failReason = failReason
			};
		}

		// Token: 0x04005856 RID: 22614
		public bool canUse;

		// Token: 0x04005857 RID: 22615
		public string failReason;
	}
}
