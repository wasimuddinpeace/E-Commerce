/// <binding AfterBuild='minify' />
var gulp = require('gulp');
var uglify = require("gulp-uglify");
var concat = require("gulp-concat");

gulp.task("minify", function () {
    return gulp.src("wwwroot/js/**/*.js")
        .pipe(uglify()) //minify each file separate ly inside js 
        .pipe(concat("dutchtreat.min.js")) 
        .pipe(gulp.dest("wwwroot/dist")); //take this file modifiy them asnd save them in dist
});

//for multiple tasks we have []
gulp.task('default', ["minify"]);