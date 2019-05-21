﻿import Vue from 'vue'
import Router from 'vue-router'
import Books from '@/components/Books'
import Orders from '@/components/Orders'

Vue.use(Router)

export default new Router({
    routes: [
        {
            path: '/',
            name: 'Books',
            component: Books
        },
        {
            path: '/orders',
            name: 'Orders',
            component: Orders
        }
    ]
})