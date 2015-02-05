using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace Blackriverinc.Framework.Utility
	{
	public interface IHandleable
		{
		bool Handled { get; set; }
		}


	/// <summary>
	/// 
	/// </summary>
	public class HandleableException :  Exception, IHandleable
		{
		#region IHandleable

		int _handled = 0;

		/// <summary>
		/// true :	ExceptionReport has been generated and published to
		///			subscribers on 'OnStreamDone'.
		/// </summary>
		public bool Handled
			{
			get
				{
				int handled = 0;
				Interlocked.Exchange(ref handled, _handled);
				return (handled != 0);
				}

			set
				{
				Interlocked.Exchange(ref _handled, 1);
				}
			}

		#endregion

		public HandleableException(Exception exp) : base(exp.Message, exp)
			{
			}

		public HandleableException(string message, Exception exp)
			: base(message, exp)
			{
			}

		public override string ToString()
			{
			return this.InnerException.ToString();
			}

		public override string StackTrace
			{
			get
				{
				return this.InnerException.StackTrace;
				}
			}
		}

	public delegate void ExceptionReportedDelegate(object sender, Exception exp);

	}
