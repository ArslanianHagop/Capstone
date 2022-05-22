namespace Capstone1.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLanguage>().HasKey(ul => new { ul.UserId, ul.LanguageId });

            modelBuilder.Entity<PostInterest>().HasKey(pi => new { pi.PostId, pi.InterestId });

            modelBuilder.Entity<StudentInterest>().HasKey(si => new { si.StudentId, si.InterestId });

            modelBuilder.Entity<Student>().HasKey(s => new { s.StudentId });

            modelBuilder.Entity<Teacher>().HasKey(t => new { t.TeacherId });

            modelBuilder.Entity<TeacherInterest>().HasKey(ti => new { ti.TeacherId, ti.InterestId });

            modelBuilder.Entity<TeacherCourseInterest>().HasKey(tci => new { tci.TeacherCourseId, tci.InterestId });
        }
        //For any user
        public DbSet<User> Users { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<UserLanguage> UserLanguages { get; set; }
        //For any student
        public DbSet<Student> Students { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<StudentInterest> StudentInterests { get; set; }
        public DbSet<PostInterest> PostInterests { get; set; }
		public DbSet<Tag> Tags { get; set; }
        //For any teacher
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<TeacherInterest> TeacherInterests { get; set; }
        public DbSet<TeacherCourse> TeacherCourses { get; set; }
        public DbSet<TeacherCourseInterest> TeacherCourseInterests { get; set; }
        public DbSet<TeacherJob> TeacherJobs { get; set; }
        //For Chat
        public DbSet<Chat> Chats { get; set; }
    }
}
