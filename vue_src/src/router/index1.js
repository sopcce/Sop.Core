export const asyncRoutes = [
  {
    path: '/managener',
    name: 'Managener',
    meta: {
      title: '各平台管理',
      icon: 'example',
      roles: ['admin', 'editor']
    }
  },

  {
    path: 'funManagener',
    name: 'FunManagener',
    meta: {
      title: '回收系统功能管理',
      icon: 'tree',
      roles: ['admin']
    }
  },

  {
    path: 'acManagener',
    name: 'AcManagener',
    meta: {
      title: '回收系统账套管理',
      icon: 'table',
      roles: ['admin', 'editor']
    }
  },

  {
    path: 'screenModule',
    name: 'ScreenModule',
    meta: {
      title: '可视化大屏模板管理',
      icon: 'table',
      roles: ['admin', 'editor']
    }
  },

  {
    path: 'screenAccount',
    name: 'ScreenAccount',
    meta: {
      title: '可视化大屏账号管理',
      icon: 'table',
      roles: ['admin', 'editor']
    }
  },

  {
    path: 'acMangentnewPlate',
    name: 'AcMangentnewPlate',
    meta: {
      title: ' 新增平台',
      roles: ['admin', 'editor']
    }
  },

  {
    path: 'acMangentdetail/:id',
    name: 'AcMangentdetail',
    meta: {
      title: ' 详情',
      roles: ['admin']
    }
  },

  {
    path: 'acMangentedit/:id',
    name: 'AcMangentedit',
    meta: {
      title: ' 编辑',
      roles: ['admin', 'editor']
    }
  },

  {
    path: 'acMangentepeizhi/:id',
    name: 'AcMangentepeizhi',
    meta: {
      title: ' 配置功能 ',
      roles: ['admin']
    }
  },
  {
    path: '/system',
    name: 'System',
    meta: {
      title: '系统管理',
      icon: 'example',
      roles: ['admin']
    }
  },

  {
    path: 'rolesManagener',
    name: 'RolesManagener',
    meta: { title: '用户权限管理', icon: 'tree', roles: ['admin'] }
  },

  {
    path: 'userManagener',
    name: 'UserManagener',
    meta: { title: '用户管理', icon: 'table', roles: ['admin'] }
  },

  // 404 page must be placed at the end !!!
  { path: '*', redirect: '/404', hidden: true }
]
