// 完整引入 Element
import Vue from 'vue'
import ElementUI from 'element-ui'
import locale from 'element-ui/lib/locale/lang/en'
import axios from 'axios';
Vue.prototype.axios = axios;

Vue.use(ElementUI, { locale });
 



