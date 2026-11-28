using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000549 RID: 1353
	public abstract class AttachableThing : ThingWithComps, IThingGlower
	{
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x060027FC RID: 10236 RVA: 0x000F70C3 File Offset: 0x000F52C3
		public override Vector3 DrawPos
		{
			get
			{
				if (this.parent != null)
				{
					return this.parent.DrawPos + Vector3.up * 0.03292683f;
				}
				return base.DrawPos;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x060027FD RID: 10237
		public abstract string InspectStringAddon { get; }

		// Token: 0x060027FE RID: 10238 RVA: 0x000F70F3 File Offset: 0x000F52F3
		public virtual bool ShouldBeLitNow()
		{
			return this.parent != null;
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x000F70FE File Offset: 0x000F52FE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.parent, "parent", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.parent != null)
			{
				this.AttachTo(this.parent);
			}
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000F7134 File Offset: 0x000F5334
		public virtual void AttachTo(Thing newParent)
		{
			this.parent = newParent;
			CompAttachBase compAttachBase;
			if (!newParent.TryGetComp(out compAttachBase))
			{
				Log.Error(string.Concat(new string[]
				{
					"Cannot attach ",
					(this != null) ? this.ToString() : null,
					" to ",
					(newParent != null) ? newParent.ToString() : null,
					": parent has no CompAttachBase."
				}));
				return;
			}
			compAttachBase.AddAttachment(this);
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000F71A2 File Offset: 0x000F53A2
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.parent != null)
			{
				this.parent.TryGetComp<CompAttachBase>().RemoveAttachment(this);
			}
		}

		// Token: 0x04001C56 RID: 7254
		public Thing parent;
	}
}
