using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000262 RID: 610
	public interface IFleck
	{
		// Token: 0x060011C3 RID: 4547
		void Setup(FleckCreationData creationData);

		// Token: 0x060011C4 RID: 4548
		bool TimeInterval(float deltaTime, Map map);

		// Token: 0x060011C5 RID: 4549
		void Draw(DrawBatch batch);

		// Token: 0x060011C6 RID: 4550
		Vector3 GetPosition();
	}
}
