//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoffeeShopLMS.DATA.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class QuizQuestion
    {
        public int QuestionID { get; set; }
        public int LessonID { get; set; }
        public string Question { get; set; }
        public string RightAnswer { get; set; }
        public string WrongAnswer1 { get; set; }
        public string WrongAnswer2 { get; set; }
        public string WrongAnswer3 { get; set; }
    
        public virtual Lesson Lesson { get; set; }
    }
}
