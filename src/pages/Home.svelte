<script lang="ts">

    const steps = 24 * 60 / 15
    const labelsCount = 48
    const primaryLabelsCount = 12
    let selected = Array(steps).fill(0, 0, steps)
    let elementWidth = 1;

    let editingStart = false;
    let editingEnd = false;
    let editingIndex = 0;
    let editingMode = 0;

    function toggleSelected(i: number) {
        selected[i] = getEditingValueForCell(i);
    }
    function applyEditing(index: number) {
        const a = Math.min(editingIndex, index);
        const b = Math.max(editingIndex, index);
        const v = getEditingValueForCell(index);
        for (let i = a; i <= b; i++) {
            selected[i] = v;
        }
    }
    function isSelected(arr: any[], val: number) {
        return arr[val] > 0;
    }
    function isStartOfRange(arr: any[], val: number) {
        return isSelected(arr, val) && (val == 0 || !isSelected(arr, val - 1));
    }
    function isEndOfRange(arr: any[], val: number) {
        return isSelected(arr, val) && (val == steps - 1 || !isSelected(arr, val + 1));
    }
    function getSpanLength(arr: any[], start: number) {
        let len = 1;
        while (isSelected(arr, start + len)) {
            len++;
        }
        return len;
    }
    function getEditingValueForCell(i: number) {
        if (!editingEnd && editingIndex <= i)
            return 0;
        if (!editingStart && editingIndex >= i)
            return 0;
        return 1;
    }
    function handleMouseOver(event: MouseEvent, i: number) {
        if (event.buttons === 1 && (editingStart || editingEnd)) {
            applyEditing(i);
        }
    }
    function handleMouseDown(event: MouseEvent, i: number) {
        editingStart = false;
        editingEnd = false;
        if (isStartOfRange(selected, i)) {
            editingStart = true;
            editingIndex = i;
        }
        if (isEndOfRange(selected, i)) {
            editingEnd = true;
            editingIndex = i;
        }
        if (!isSelected(selected, i)) {
            editingStart = true;
            editingEnd = true;
            editingIndex = i;
        }
    }
    function padTo2Digits(num: number) {
        return String(num).padStart(2, '0');
    }
    function getStepLabel(step: number) {
        const hours = (step / steps) * 24;
        const fullHours = Math.floor(hours);
        const minute = Math.round((hours % 1) * 60);
        const hoursAndMinutes = padTo2Digits(fullHours) + ':' + padTo2Digits(minute);
        return hoursAndMinutes;
    }
</script>

<div class="flex flex-row flex-wrap">
    {#each {length: steps} as _, i}
        <div bind:clientWidth={elementWidth} class="w-0 shrink grow basis-0 ">
            <div class="block w-full h-8 relative">
                {#if isStartOfRange(selected, i)}
                    <div class="rounded-lg bg-orange-500 text-white w-10 h-6">
                        { getStepLabel(i) }
                    </div>
                {/if}
                {#if isEndOfRange(selected, i)}
                    <div class="rounded-lg bg-orange-500 text-white w-10 h-6">
                        { getStepLabel(i+1) }
                    </div>
                {/if}
            </div>
            <button
                class="span-element block w-full h-16"
                class:selected={isSelected(selected, i)}
                class:range-start={isStartOfRange(selected, i)}
                class:range-end={isEndOfRange(selected, i)}
                on:mousedown={e => handleMouseDown(e, i)}    
                on:mouseenter={e => handleMouseOver(e, i)}
            >
                {#if isStartOfRange(selected, i) && getSpanLength(selected, i) > 4}
                    <div class="absolute h-full text-center text-white text-2xl z-10 pointer-events-none" 
                        style="width: {getSpanLength(selected, i) * elementWidth}px;"
                    >
                        {getStepLabel(getSpanLength(selected, i))}
                    </div>
                {/if}
            </button>
            <div
                class="label block w-full h-8 border-r-2 border-gray-300"
                class:primary={i % (steps / primaryLabelsCount) == 0 || i == steps - 1}
            >
                {#if i % (steps / primaryLabelsCount) == 0 || i == steps - 1}
                    { getStepLabel(i) }
                {/if}
            </div>
        </div>
    {/each}
</div>

<style lang="postcss">
    .span-element.selected {
        @apply bg-orange-400 h-12 mt-4;

        &.range-start {
            @apply bg-orange-500 rounded-tl-lg h-16 mt-0;
        }
        &.range-end {
            @apply bg-orange-500 rounded-tr-lg h-16 mt-0;
        }
    }

    .label {
        @apply text-gray-600 text-sm;
    }
    .label.primary {
        @apply text-gray-950 text-base;
    }
</style>