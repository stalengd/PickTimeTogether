import { writable } from 'svelte/store';
import Home from './pages/Home.svelte'

export const routes = [
	{ path: '/', label:'Home', component: Home, exact: true },
	{ path: '/home', label:'Home 2', component: Home, exact: true },
];
