using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000C6 RID: 198
	public class DeathActionProperties
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0002C5CF File Offset: 0x0002A7CF
		public DeathActionWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (Activator.CreateInstance(this.workerClass) as DeathActionWorker);
					this.workerInt.props = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0002C601 File Offset: 0x0002A801
		public virtual IEnumerable<string> ConfigErrors()
		{
			return new DeathActionProperties.<ConfigErrors>d__4(-2);
		}

		// Token: 0x04000364 RID: 868
		public Type workerClass = typeof(DeathActionWorker_Simple);

		// Token: 0x04000365 RID: 869
		private DeathActionWorker workerInt;
	}
}
