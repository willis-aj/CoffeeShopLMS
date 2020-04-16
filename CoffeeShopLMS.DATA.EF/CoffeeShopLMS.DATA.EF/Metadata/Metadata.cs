using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;//added for model metadata

namespace CoffeeShopLMS.DATA.EF
{
    #region Course Metadata
    public class CoursMetadata
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(200)]
        public string CourseName { get; set; }

        [Display(Name = "Description")]
        [StringLength(500)]
        [UIHint("MultilineText")]
        public string CourseDescription { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(CoursMetadata))]
    public partial class Cours { }
    #endregion

    #region CourseCompletion Metadata
    public class CourseCompletionMetadata
    {
        [Required]
        [Display(Name = "User ID")]
        [StringLength(128)]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "Course ID")]
        public int CourseID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime DateCompleted { get; set; }
    }

    [MetadataType(typeof(CourseCompletionMetadata))]
    public partial class CourseCompletion { }
    #endregion

    #region Lessons Metadata
    public class LessonMetadata
    {
        [Required]
        [StringLength(200)]
        [Display(Name = "Title")]
        public string LessonTitle { get; set; }

        [Required]
        public int CourseID { get; set; }

        [StringLength(300)]
        [UIHint("MultilineText")]
        public string Introduction { get; set; }

        [StringLength(250)]
        public string VideoURL { get; set; }

        [StringLength(100)]
        public string PdfFileName { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Quiz")]
        public bool QuizRequired { get; set; }

        [Display(Name = "Video Required?")]
        public bool VideoRequired { get; set; }
    }

    [MetadataType(typeof(LessonMetadata))]
    public partial class Lesson { }
    #endregion

    #region Lessons views Metadata
    public class LessonViewMetadata
    {
        [Required]
        [StringLength(128)]
        public string UserID { get; set; }

        [Required]
        public int LessonID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime DateViewed { get; set; }
    }

    [MetadataType(typeof(LessonViewMetadata))]
    public partial class LessonView { }
    #endregion

}//end namespace
