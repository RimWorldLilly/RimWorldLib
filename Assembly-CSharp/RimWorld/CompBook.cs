using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA7 RID: 3751
	public class CompBook : CompReadable
	{
		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x060062CD RID: 25293 RVA: 0x002390E7 File Offset: 0x002372E7
		public new CompProperties_Book Props
		{
			get
			{
				return (CompProperties_Book)this.props;
			}
		}

		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x060062CE RID: 25294 RVA: 0x002390F4 File Offset: 0x002372F4
		public new IEnumerable<BookOutcomeDoer> Doers
		{
			get
			{
				return this.doers.OfType<BookOutcomeDoer>();
			}
		}

		// Token: 0x060062CF RID: 25295 RVA: 0x00239101 File Offset: 0x00237301
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompBook.<CompGetGizmosExtra>d__4 <CompGetGizmosExtra>d__ = new CompBook.<CompGetGizmosExtra>d__4(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}
	}
}
