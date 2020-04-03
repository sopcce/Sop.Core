import request from '@/utils/request'

export function getRoutes() {
  return request({
    url: '/api/role/routes',
    method: 'get'
  })
}

export function getRoles() {
  return request({
    url: '/api/role/roles',
    method: 'get'
  })
}

export function addRole(data) {
  return request({
    url: '/api/role/role',
    method: 'post',
    data
  })
}

export function updateRole(id, data) {
  return request({
    url: `/api/role/${id}`,
    method: 'put',
    data
  })
}

export function deleteRole(id) {
  return request({
    url: `/api/role/${id}`,
    method: 'delete'
  })
}
