using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserMetadata
    {
        [Required(ErrorMessage = "用户名必须填写")]
        public string Username { get; set; }

        [Required(ErrorMessage = "密码必须填写")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "密码必须在8~12位之间")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "两次输入密码不一致")]
        public string ConfirmPassword { get; set; }
    }
}