<!-- <svelte:options tag="svelte-app" /> -->

<script lang="ts">
    import { Router, Route, link } from 'svelte-routing';
    import { routes } from './routes';

    let pathname = location.pathname;
</script>

<Router>
    <nav class="mb-2">
        <div class="flex">
            <a class="mr-auto text-2xl" href="/">
                <i class=""></i>
                Pick Time Together
            </a>
            <ul class="flex items-center">
            {#each routes as {path, label, exact} (label) }
                <li class="mx-2">
                    <a href="{path}" class="" class:active={exact ? pathname == path : pathname.startsWith(path)} use:link>{label}</a>
                </li>
            {/each}
            </ul>
        </div>
    </nav>
    <div class="container mx-auto bg-slate-400">
        {#each routes as {path, component}}
            <Route path="{path}" component="{component}" />
        {/each}
    </div>
</Router>

<style>
    h1 {
        font-size: 5em;
    }
</style>