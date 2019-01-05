// 完整引入 Element
//;
//import Vue from 'vue'
//import ElementUI from 'element-ui'
//import locale from 'element-ui/lib/locale/lang/en'
//import axios from 'axios';
//Vue.prototype.axios = axios;

//Vue.use(ElementUI, { locale });
app();
function app() {

    var Ctor3 = Vue.extend({
        data() {
            return {
                imageUrl: ''
            };
        },
        methods: {
            handleAvatarSuccess(res, file) {
                this.imageUrl = URL.createObjectURL(file.raw);
            },
            beforeAvatarUpload(file) {
                debugger
                const isJPG = file.type === 'image/jpeg';
                const isLt2M = file.size / 1024 / 1024 < 2;

                if (!isJPG) {
                    this.$message.error('上传头像图片只能是 JPG 格式!');
                }
                if (!isLt2M) {
                    this.$message.error('上传头像图片大小不能超过 2MB!');
                }
                return isJPG && isLt2M;
            }
        }
    });
    new Ctor3().$mount('#ProfileHeader-wrapper');
}



