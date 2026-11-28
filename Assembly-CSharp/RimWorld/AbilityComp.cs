using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013CA RID: 5066
	public abstract class AbilityComp
	{
		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x06007E03 RID: 32259 RVA: 0x002D119C File Offset: 0x002CF39C
		public virtual bool CanCast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007E04 RID: 32260 RVA: 0x002D119F File Offset: 0x002CF39F
		public virtual void Initialize(AbilityCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06007E05 RID: 32261 RVA: 0x002D11A8 File Offset: 0x002CF3A8
		public override string ToString()
		{
			string[] array = new string[6];
			array[0] = base.GetType().Name;
			array[1] = "(parent=";
			int num = 2;
			Ability ability = this.parent;
			array[num] = ((ability != null) ? ability.ToString() : null);
			array[3] = " at=";
			array[4] = ((this.parent != null) ? this.parent.pawn.Position : IntVec3.Invalid).ToString();
			array[5] = ")";
			return string.Concat(array);
		}

		// Token: 0x06007E06 RID: 32262 RVA: 0x002D122B File Offset: 0x002CF42B
		public virtual bool GizmoDisabled(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x06007E07 RID: 32263 RVA: 0x002D1231 File Offset: 0x002CF431
		public virtual float PsyfocusCostForTarget(LocalTargetInfo target)
		{
			return 0f;
		}

		// Token: 0x06007E08 RID: 32264 RVA: 0x002D1238 File Offset: 0x002CF438
		public virtual void CompTick()
		{
		}

		// Token: 0x06007E09 RID: 32265 RVA: 0x002D123A File Offset: 0x002CF43A
		public virtual void CompTickInterval(int delta)
		{
		}

		// Token: 0x06007E0A RID: 32266 RVA: 0x002D123C File Offset: 0x002CF43C
		public virtual IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06007E0B RID: 32267 RVA: 0x002D1243 File Offset: 0x002CF443
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06007E0C RID: 32268 RVA: 0x002D1246 File Offset: 0x002CF446
		public virtual void PostExposeData()
		{
		}

		// Token: 0x04004F31 RID: 20273
		public Ability parent;

		// Token: 0x04004F32 RID: 20274
		public AbilityCompProperties props;
	}
}
