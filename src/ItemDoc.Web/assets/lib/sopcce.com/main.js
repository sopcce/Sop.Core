// 完整引入 Element
import Vue from 'vue'
import ElementUI from 'element-ui'
import locale from 'element-ui/lib/locale/lang/en'
import axios from 'axios';
Vue.prototype.axios = axios;

Vue.use(ElementUI, { locale });

new Vue({
    el: '#app',
    data: function () {
        return { visible: false }
    }
})

var Ctor1 = Vue.extend({
    data() {
        return {
            dialogImageUrl: '',
            dialogVisible: false
        };
    },
    methods: {
        handleRemove(file, fileList) {
            console.log(file, fileList);
        },
        handlePictureCardPreview(file) {
            debugger
            console.log(file);
            this.dialogImageUrl = file.url;
            this.dialogVisible = true;
        }
    }
});
new Ctor1().$mount('#app_1');

var Ctor2 = Vue.extend({
    data() {
        return {
            fileList2: [{
                name: 'food.jpeg',
                url: 'https://fuss10.elemecdn.com/3/63/4e7f3a15429bfda99bce42a18cdd1jpeg.jpeg?imageMogr2/thumbnail/360x360/format/webp/quality/100'
            }, {
                name: 'food2.jpeg',
                url: 'https://fuss10.elemecdn.com/3/63/4e7f3a15429bfda99bce42a18cdd1jpeg.jpeg?imageMogr2/thumbnail/360x360/format/webp/quality/100'
            }]
        };
    },
    methods: {
        handleRemove(file, fileList) {
            console.log(file, fileList);
        },
        handlePreview(file) {
            console.log(file);
        }
    }
});
new Ctor2().$mount('#app_2');