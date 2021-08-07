using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLC.SSP
{
	class Utils
	{
		public static Rotation FromToRotation( Vector3 aFrom, Vector3 aTo )
		{
			Vector3 axis = Vector3.Cross( aFrom, aTo );
			float angle = Vector3.GetAngle( aFrom, aTo );
			return Rotation.FromAxis( axis.Normal, angle );
		}
	}
}
