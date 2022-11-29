using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeriLogDemo_Net6.Model
{

	/// <summary>
	/// t_agent 模型
	/// </summary>
	public class t_agent
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string agent_token { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string agent_key { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string agent_name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public short agent_status { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime create_datetime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime update_datetime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string meta { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string agent_nickname { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int agent_group { get; set; }
	}

}